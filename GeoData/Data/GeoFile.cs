﻿using System;
using GeoData.Models;

namespace GeoData.Data
{
    public abstract class GeoFile : IGeoFile, IDisposable
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
            // Прямой перебор
            var t0 = DateTime.Now;
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

            var result = new SearchResult(location, t0);
            return result;
        }

        public SearchResult FindLocationByIp(string ip)
        {
            var t0 = DateTime.Now;
            BaseGeoLocation location = null;

            if (uint.TryParse(ip, out var ipValue))
            {
                lock (file)
                {
                    // Прямой перебор
                    for (uint i = 0; i < Header.Records; i++)
                    {
                        var item = GetIpRangeAt(i);
                        if (item.IpFrom <= ipValue && ipValue <= item.IpTo)
                        {
                            location = GetLocationAt(item.LocationIndex);
                            break;
                        }
                    }
                }
            }

            var result = new SearchResult(location, t0);
            return result;
        }

        //метод бинарного поиска с использованием цикла
        static long BinarySearchIp(IGeoFile geoFile, uint ipValue, uint left, uint right)
        {
            //пока не сошлись границы массива
            while (left <= right)
            {
                //индекс среднего элемента
                var middle = (left + right) / 2;

                if (ipValue == geoFile.GetIpRangeAt(middle).IpFrom)
                {
                    return middle;
                }
                else if (ipValue < geoFile.GetIpRangeAt(middle).IpFrom)
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
