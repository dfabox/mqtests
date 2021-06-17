using GeoData.Models;

namespace GeoData.Data
{
    public interface IGeoFile
    {
        public BaseHeader Header { get; }

        public BaseGeoLocation GetLocationAt(uint index);
        public BaseIpRange GetIpRangeAt(uint index);
        public BaseCityIndex GetCityIndexAt(uint index);

        public SearchResult GeoLocationByIp(string ip);
        public SearchResult GeoLocationByCity(string city);
    }
}
