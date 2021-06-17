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

        public BaseGeoPosition GetPositionAt(uint index)
        {
            if (index < 0 || index >= data.Header.Records)
                throw new IndexOutOfRangeException();

            var buffer = data.ReadBuffer(Header.OffsetLocations + index* BaseGeoPosition.SIZE, BaseGeoPosition.SIZE);

            if (buffer == null)
                throw new NullReferenceException();

            var result = new BaseGeoPosition(buffer);

            return result;
        }

        public SearchResult GeoPositionFromCity(string city)
        {
            throw new NotImplementedException();
        }

        public SearchResult GeoPositionFromIp(string ip)
        {
            throw new NotImplementedException();
        }
    }
}
