using System;
using System.Collections.Generic;
using System.Diagnostics;
using GeoData.Common;
using GeoData.Models;
using static GeoData.Base.BaseConsts;

namespace GeoData.Data
{
    /// <summary>
    /// Абстрактная реализация механизмов поиска и получения данных на основе чтения буфера данных
    /// </summary>
    public abstract class GeoBase : IGeoBase, IDisposable
    {
        public BaseHeader Header { get; private set; }
        protected IDisposable file;

        protected void LoadHeader()
        {
            var buffer = ReadBuffer(0, BaseHeader.SIZE);
            Header = new BaseHeader(buffer);
        }

        protected virtual byte[] ReadBuffer(uint offset, uint count)
        {
            throw new NotImplementedException();
        }

        private byte[] GetBufferAt(uint index, uint offset, uint size)
        {
            if (index < 0 || index >= Header.Records)
                throw new IndexOutOfRangeException();

            var buffer = ReadBuffer(offset + index * size, size);

            if (buffer == null)
                throw new NullReferenceException();

            return buffer;
        }

        public BaseGeoLocation GetLocationAt(uint index)
        {
            var buffer = GetBufferAt(index, Header.OffsetLocations, BaseGeoLocation.SIZE);

            return new BaseGeoLocation(buffer);
        }

        private BaseGeoLocation GetLocationFromAddress(uint address)
        {
            var buffer = ReadBuffer(Header.OffsetLocations + address, BaseGeoLocation.SIZE);

            return new BaseGeoLocation(buffer);
        }

        public BaseIpRange GetIpRangeAt(uint index)
        {
            var buffer = GetBufferAt(index, Header.OffsetRanges, BaseGeoLocation.SIZE);

            return new BaseIpRange(buffer);
        }

        private uint GetCityAddressAt(uint index)
        {
            var buffer = GetBufferAt(index, Header.OffsetCities, 4);

            return BitConverter.ToUInt32(buffer, 0);
        }

        private string GetCityFromAddress(uint address)
        {
            var buffer = ReadBuffer(Header.OffsetLocations + address + BaseGeoLocation.CITY_OFFSET, BaseGeoLocation.CITY_SIZE);
            var result = buffer.GetStringFromBytes(0, Convert.ToInt32(BaseGeoLocation.CITY_SIZE));

            return result;
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
                    var address = GetCityAddressAt(i);
                    var location = GetLocationFromAddress(address);
                    locations.Add(location);
                }
            }

            sw.Stop();
            var result = new SearchResult(locations, sw);
            return result;
        }

        private BaseIpRange FindRangeByIp(uint ip)
        {
            var index = BinarySearchIp(ip, 0, Convert.ToUInt32(IP_RANGE_COUNT - 1));

            if (index >= 0)
                return GetIpRangeAt(Convert.ToUInt32(index));
            else
                return null;
        }

        public SearchResult FindLocationByIp(uint ip)
        {
            var sw = Stopwatch.StartNew();

            BaseGeoLocation location = null;

            var ipRange = FindRangeByIp(ip);

            if (ipRange != null)
                location = GetLocationAt(ipRange.LocationIndex);

            sw.Stop();
            var result = new SearchResult(location, sw);
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

                ipRange = GetIpRangeAt(middle);
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

        /// <summary>
        /// Сравнение заданного названия города с городом на который указывает заданный индекс
        /// </summary>
        /// <param name="city">название города</param>
        /// <param name="index">позиция в индексе</param>
        /// <returns>результат сравнения строк</returns>
        private int EqualCityAt(string city, uint index)
        {
            var address = GetCityAddressAt(index);
            var city1 = GetCityFromAddress(address);

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

        #region IDisposable
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                file?.Dispose();
                file = null;
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
