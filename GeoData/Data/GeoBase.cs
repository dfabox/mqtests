using System;
using System.Diagnostics;
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

            // TODO Возможно потребуется оптимизация "конвертации" буфера в структуру элемента
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

        public SearchResult FindLocationByCity(string city)
        {
            var sw = Stopwatch.StartNew();

            BaseGeoLocation location = null;

            lock (file)
            {
                for (uint i = 0; i < Header.Records; i++)
                {
                    var item = GetLocationAt(i);
                    if (item.City == city)
                    {
                        location = item;
                        break;
                    }
                }
            }

            sw.Stop();
            var result = new SearchResult(location, sw);
            return result;
        }

        public BaseIpRange FindRangeByIp(uint ip)
        {
            lock (file)
            {
                var index = BinarySearchIp(this, ip, 0, Convert.ToUInt32(IP_RANGE_COUNT - 1));

                if (index >= 0)
                    return GetIpRangeAt(Convert.ToUInt32(index));
                else
                    return null;
            }
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

        //метод бинарного поиска с использованием цикла
        private static long BinarySearchIp(IGeoBase geoFile, uint ipValue, uint left, uint right)
        {
            // Последний просмотренный элемент
            BaseIpRange ipRange;

            // Пока не сошлись границы массива
            while (left <= right)
            {
                // Индекс среднего элемента
                var middle = (left + right) / 2;

                ipRange = geoFile.GetIpRangeAt(middle);
                // Не точное значение, а попадание в диапазон
                if (ipRange.IpFrom <= ipValue && ipValue <= ipRange.IpTo)
                {
                    return middle;
                }
                else if (ipValue < ipRange.IpFrom)
                {
                    //сужаем рабочую зону массива с правой стороны
                    right = middle - 1;
                }
                else
                {
                    //сужаем рабочую зону массива с левой стороны
                    left = middle + 1;
                }
            }
            // Или нашли точное совпадение с IpFrom или диапазон включающий

            //ничего не нашли
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
