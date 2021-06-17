using GeoData.Models;

namespace GeoData.Search
{
    public class SearchResult
    {
        public SearchResultStatus Status { get; set; }
        public string Msg { get; set; }
        public BaseGeoLocation Location { get; set; }

        public SearchResult(string errorMsg)
        {
            Status = SearchResultStatus.Error;
            Msg = errorMsg;
        }

        public SearchResult(BaseGeoLocation location)
        {
            Status = location == null ? SearchResultStatus.NotFound : SearchResultStatus.Success;
            Location = location;
        }
    }
}
