using GeoData.Models;

namespace GeoData.Search
{
    public interface IGeoSearch
    {
        public SearchResult GeoPositionFromIp(string ip);
        public SearchResult GeoPositionFromCity(string city);
    }
}
