using GeoData.Models;

namespace GeoData.Data
{
    public interface IGeoBase
    {
        public BaseHeader Header { get; }

        public BaseGeoLocation GetLocationAt(uint index);
        public BaseIpRange GetIpRangeAt(uint index);
        public BaseCityIndex GetCityIndexAt(uint index);

        public BaseIpRange FindRangeByIp(uint ip);
        public SearchResult FindLocationByIp(uint ip);
        public SearchResult FindLocationByCity(string city);
    }
}
