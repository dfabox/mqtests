using System;
using System.Collections.Generic;
using System.Diagnostics;
using GeoData.Common;
using GeoData.Models;

namespace GeoData.Data
{
    public class GeoSearch : IGeoSearch
    {
        private IGeoBase geoBase;
        private BaseHeader Header => geoBase.Header;

        public GeoSearch(IGeoBase geoBase)
        {
            if (geoBase == null)
                throw new GeoBaseInitializeErrorException("данные не инициализированы");

            this.geoBase = geoBase;
        }

        public SearchResult FindLocationByCity(string city)
        {
            var sw = Stopwatch.StartNew();

            ICollection<BaseGeoLocation> locations = null;

            // TODO Оптимизировать сравнением не строк а массивов?
            // Результат - индекс адреса, НЕ местоположения
            var index = BinarySearchCity(city, 0, Convert.ToUInt32(Header.Records - 1));
            if (index >= 0)
            {
                // Найден один город, но могут быть повторы вверх или вниз по индексу
                // Найти минимальный и максимальный индексы для найденного города
                var index1 = Convert.ToUInt32(index - 1);
                while (index1 > 0 && EqualCityAt(city, Convert.ToUInt32(index1)) == 0)
                    index1--;
                var minIndex = index1 + 1;

                index1 = Convert.ToUInt32(index + 1);
                while (index1 < Header.Records && EqualCityAt(city, Convert.ToUInt32(index1)) == 0)
                    index1++;
                var maxIndex = index1 - 1;

                locations = new List<BaseGeoLocation>();
                for (var i = minIndex; i <= maxIndex; i++)
                {
                    var address = geoBase.GetCityAddressAt(i);
                    var location = geoBase.GetLocationFromAddress(address);
                    locations.Add(location);
                }
            }

            sw.Stop();
            var result = new SearchResult(locations, sw);
            return result;
        }

        private BaseIpRange FindRangeByIp(uint ip)
        {
            var index = BinarySearchIp(ip, 0, Convert.ToUInt32(Header.Records - 1));

            if (index >= 0)
                return geoBase.GetIpRangeAt(Convert.ToUInt32(index));
            else
                return null;
        }

        public SearchResult FindLocationByIp(uint ip)
        {
            var sw = Stopwatch.StartNew();

            BaseGeoLocation location = null;

            var ipRange = FindRangeByIp(ip);

            if (ipRange != null)
                location = geoBase.GetLocationAt(ipRange.LocationIndex);

            sw.Stop();
            var result = new SearchResult(location, sw)
            {
                IpRange = ipRange
            };
            return result;
        }

        // Бинарный поиск ip
        private long BinarySearchIp(uint ipValue, uint left, uint right)
        {
            // TODO Можно оптимизировать, если при поиске читать не весь объект, а только границы
            BaseIpRange ipRange;

            // Пока не сошлись границы массива
            while (left <= right)
            {
                // Индекс среднего элемента
                var middle = (left + right) / 2;

                ipRange = geoBase.GetIpRangeAt(middle);
                // Не точное значение, а попадание в диапазон
                if (ipRange.IpFrom <= ipValue && ipValue <= ipRange.IpTo)
                {
                    return middle;
                }
                else if (ipValue < ipRange.IpFrom)
                {
                    // Сужаем рабочую зону массива с правой стороны
                    right = middle - 1;
                }
                else
                {
                    // Сужаем рабочую зону массива с левой стороны
                    left = middle + 1;
                }
            }
            // Ничего не нашли
            return -1;
        }

        // Сравнение заданного названия города с городом на который указывает заданный индекс
        private int EqualCityAt(string city, uint index)
        {
            var address = geoBase.GetCityAddressAt(index);
            var city1 = geoBase.GetCityFromAddress(address);

            var compare = string.Compare(city, city1);

            return compare;
        }

        // Бинарный поиск города
        private long BinarySearchCity(string city, uint left, uint right)
        {
            // Пока не сошлись границы массива
            while (left <= right)
            {
                // Индекс среднего элемента
                var middle = (left + right) / 2;

                var compare = EqualCityAt(city, middle);
                if (compare == 0)
                {
                    return middle;
                }
                else if (compare < 0)
                {
                    // Сужаем рабочую зону массива с правой стороны
                    right = middle - 1;
                }
                else
                {
                    // Сужаем рабочую зону массива с левой стороны
                    left = middle + 1;
                }
            }
            // Ничего не нашли
            return -1;
        }

        public ICollection<string> GetRandomIp(int count = 10)
        {
            // Список номера ip по случайному индексу
            var random = new Random();
            var result = new List<string>();

            // Сформировать заданное количество городов для поиска
            for (var i = 0; i < count; i++)
            {
                var index = Convert.ToUInt32(random.Next(geoBase.Header.Records));
                var ipRange = geoBase.GetIpRangeAt(index);

                result.Add((ipRange.IpFrom + 1).ToString());
            }

            return result;
        }

        public ICollection<string> GetRandomCity(int count = 10)
        {
            // Список города по случайному индексу
            var random = new Random();
            var result = new List<string>();

            // Сформировать заданное количество ip для поиска
            for (var i = 0; i < count; i++)
            {
                var index = Convert.ToUInt32(random.Next(geoBase.Header.Records));
                var city = geoBase.GetLocationAt(index)?.City;

                result.Add(city);
            }

            return result;
        }
    }
}
