using System;
using System.Diagnostics;
using GeoData.Models;

namespace GeoData.Data
{
    public class SearchResult
    {
        public SearchResultStatus Status { get; set; }
        public string Msg { get; set; }
        public BaseGeoLocation Location { get; set; }
        public double TimeMs { get; set; }

        public SearchResult(string errorMsg)
        {
            Status = SearchResultStatus.Error;
            Msg = errorMsg;
        }

        public SearchResult(BaseGeoLocation location, Stopwatch sw)
        {
            Status = location == null ? SearchResultStatus.NotFound : SearchResultStatus.Success;
            Location = location;
            TimeMs = sw.Elapsed.TotalMilliseconds;
        }
    }
}
