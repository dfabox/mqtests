using System;
using GeoData.Common;
using GeoData.Models;

namespace GeoData.Data
{
    /// <summary>
    /// Абстрактная реализация базы местоположения на основе чтения буфера данных потока/файла
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

        public BaseGeoLocation GetLocationFromAddress(uint address)
        {
            var buffer = ReadBuffer(Header.OffsetLocations + address, BaseGeoLocation.SIZE);

            return new BaseGeoLocation(buffer);
        }

        public BaseIpRange GetIpRangeAt(uint index)
        {
            var buffer = GetBufferAt(index, Header.OffsetRanges, BaseIpRange.SIZE);

            return new BaseIpRange(buffer);
        }

        public uint GetCityAddressAt(uint index)
        {
            var buffer = GetBufferAt(index, Header.OffsetCities, 4);

            return BitConverter.ToUInt32(buffer, 0);
        }

        public string GetCityFromAddress(uint address)
        {
            var buffer = ReadBuffer(Header.OffsetLocations + address + BaseGeoLocation.CITY_OFFSET, BaseGeoLocation.CITY_SIZE);
            var result = buffer.GetStringFromBytes(0, Convert.ToInt32(BaseGeoLocation.CITY_SIZE));

            return result;
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
