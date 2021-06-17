using System;
using GeoData.Data;
using GeoData.Models;

namespace GeoData.Search
{
    public class GeoSearch : IGeoSearch
    {
        private IGeoFile data;
        protected BaseHeader Header => data?.Header;

        public GeoSearch(IGeoFile data)
        {
            this.data = data;
        }

        public byte[] GetBufferAt(uint index, uint offset, uint size)
        {
            if (index < 0 || index >= data.Header.Records)
                throw new IndexOutOfRangeException();

            var buffer = data.ReadBuffer(offset + index * size, size);

            if (buffer == null)
                throw new NullReferenceException();

            return buffer;
        }

        public BaseGeoLocation GetLocationAt(uint index)
        {
            var buffer = GetBufferAt(index, Header.OffsetLocations, BaseGeoLocation.SIZE);

            return new BaseGeoLocation(buffer);
        }

        public BaseIpRange GetIpAt(uint index)
        {
            var buffer = GetBufferAt(index, Header.OffsetLocations, BaseGeoLocation.SIZE);

            return new BaseIpRange(buffer);
        }

        public BaseCityIndex GetCityIndexAt(uint index)
        {
            var buffer = GetBufferAt(index, Header.OffsetLocations, BaseGeoLocation.SIZE);

            return new BaseCityIndex(buffer);
        }

        public SearchResult GeoLocationByCity(string city)
        {
            throw new NotImplementedException();
        }

        public SearchResult GeoLocationByIp(string ip)
        {
            throw new NotImplementedException();
        }
    }
}
