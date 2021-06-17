using System;
using System.IO;
using GeoData.Models;

namespace GeoData.Data
{
    /// <summary>
    /// Абстрактная реализация файла базы
    /// </summary>
    public abstract class GeoFile : IGeoFile, IDisposable
    {
        private Stream stream;
        protected Stream Stream
        {
            get
            {
                if (stream == null)
                {
                    stream = GetStream();
                }

                return stream;
            }
        }

        private BaseHeader header;
        public BaseHeader Header
        {
            get
            {
                if (header == null)
                {
                    var buffer = ReadBuffer(0, BaseHeader.SIZE);
                    header = new BaseHeader(buffer);
                }

                return header;
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                if (stream != null)
                {
                    stream.Dispose();
                    stream = null;
                }
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract Stream GetStream();

        private byte[] ReadBuffer(uint offset, uint size)
        {
            // TODO Реализовать проверку параметров для чтения буфера

            var data = Stream;

            // TODO Реализовать безопасное чтение буфера из разных нитей
            lock (stream)
            {
                var reader = new BinaryReader(data);
                reader.BaseStream.Seek(offset, SeekOrigin.Begin);

                return reader.ReadBytes(Convert.ToInt32(size));
            }
        }

        /// <summary>
        /// Чтение буфера элемента из заданного блока
        /// </summary>
        /// <param name="index">индекс элемента в блоке</param>
        /// <param name="offset">смещение блока</param>
        /// <param name="size">размер элемента</param>
        /// <returns></returns>
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

            lock (stream)
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

            if (uint.TryParse(ip, out var ipInt))
            {
                lock (stream)
                {
                    // Прямой перебор
                    for (uint i = 0; i < Header.Records; i++)
                    {
                        var item = GetIpRangeAt(i);
                        if (item.IpFrom <= ipInt && ipInt <= item.IpTo)
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
    }
}
