using GeoData.Models;

namespace GeoData.Data
{
    public interface IGeoFile
    {
        public BaseHeader Header { get; }

        public BaseGeoLocation GetLocationAt(uint index);
        public BaseIpRange GetIpRangeAt(uint index);
        public BaseCityIndex GetCityIndexAt(uint index);

        public SearchResult FindLocationByIp(string ip);
        public SearchResult FindLocationByCity(string city);
    }
}
