using System;
using GeoData.Data;
using GeoData.Models;

namespace GeoData.Search
{
    public class GeoSearch : IGeoSearch
    {
        private IGeoFile data;

        public GeoSearch(IGeoFile data)
        {
            this.data = data;
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
