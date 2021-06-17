using GeoData.Models;

namespace GeoData.Search
{
    public class SearchResult
    {
        public SearchResultStatus Status { get; set; }
        public string Msg { get; set; }
        public BaseGeoPosition Position { get; set; }

        public SearchResult(string errorMsg)
        {
            Status = SearchResultStatus.Error;
            Msg = errorMsg;
        }

        public SearchResult(BaseGeoPosition position)
        {
            Status = position == null ? SearchResultStatus.NotFound : SearchResultStatus.Success;
            this.Position = position;
        }
    }
}
