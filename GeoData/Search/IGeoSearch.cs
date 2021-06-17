using GeoData.Models;

namespace GeoData.Search
{
    public interface IGeoSearch
    {
        public SearchResult GeoLocationByIp(string ip);
        public SearchResult GeoLocationByCity(string city);
    }
}
