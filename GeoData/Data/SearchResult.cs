using System;
using GeoData.Models;

namespace GeoData.Data
{
    public class SearchResult
    {
        public SearchResultStatus Status { get; set; }
        public string Msg { get; set; }
        public BaseGeoLocation Location { get; set; }
        public double TimeMS { get; set; }

        public SearchResult(string errorMsg)
        {
            Status = SearchResultStatus.Error;
            Msg = errorMsg;
        }

        public SearchResult(BaseGeoLocation location, DateTime start)
        {
            Status = location == null ? SearchResultStatus.NotFound : SearchResultStatus.Success;
            Location = location;
            TimeMS = (DateTime.Now - start).TotalMilliseconds;
        }
    }
}
