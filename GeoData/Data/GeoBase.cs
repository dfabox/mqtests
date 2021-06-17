using System;
using System.Diagnostics;
using GeoData.Common;
using GeoData.Models;
using static GeoData.Base.BaseConsts;

namespace GeoData.Data
{
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

        public BaseIpRange GetIpRangeAt(uint index)
        {
            var buffer = GetBufferAt(index, Header.OffsetRanges, BaseGeoLocation.SIZE);

            return new BaseIpRange(buffer);
        }

        public BaseCityIndex GetCityIndexAt(uint index)
        {
            var buffer = GetBufferAt(index, Header.OffsetCities, BaseGeoLocation.SIZE);

            return new BaseCityIndex(buffer);
        }

        public uint GetCityAddressAt(uint index)
        {
            var buffer = GetBufferAt(index, Header.OffsetCities, 4);

            return BitConverter.ToUInt32(buffer, 0);
        }

        public string GetCityFromAddress(uint address)
        {
            var buffer = ReadBuffer(Header.OffsetLocations + address + BaseGeoLocation.CITY_OFFSET, BaseGeoLocation.CITY_SIZE);

            return buffer.GetStringFromBytes(0, Convert.ToInt32(BaseGeoLocation.CITY_SIZE));
        }

        public SearchResult FindLocationByCity(string city)
        {
            var sw = Stopwatch.StartNew();

            BaseGeoLocation location = null;
            long index;
            lock (file)
            {
                // TODO Оптимизировать сравнением не строк а массивов?
                index = BinarySearchCity(this, city, 0, Convert.ToUInt32(Header.Records - 1));
            }
            if (index >= 0)
                location = GetLocationAt(Convert.ToUInt32(index));

            sw.Stop();
            var result = new SearchResult(location, sw);
            return result;
        }

        public BaseIpRange FindRangeByIp(uint ip)
        {
            long index;
            lock (file)
            {
                index = BinarySearchIp(this, ip, 0, Convert.ToUInt32(IP_RANGE_COUNT - 1));
            }

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
        private static long BinarySearchIp(IGeoBase geoBase, uint ipValue, uint left, uint right)
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

        // Бинарный поиск города
        private static long BinarySearchCity(IGeoBase geoBase, string city, uint left, uint right)
        {
            // Пока не сошлись границы массива
            while (left <= right)
            {
                // Индекс среднего элемента
                var middle = (left + right) / 2;

                var address = geoBase.GetCityAddressAt(middle);
                var city1 = geoBase.GetCityFromAddress(address);

                var compare = string.Compare(city, city1);
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
