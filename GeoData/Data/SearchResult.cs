using System;
using System.Collections.Generic;
using System.Diagnostics;
using GeoData.Models;

namespace GeoData.Data
{
    public class SearchResult
    {
        public SearchResultStatus Status { get; private set; }
        public string Msg { get; private set; }
        public ICollection<BaseGeoLocation> Locations { get; private set; }
        public double TimeMs { get; private set; }

        public SearchResult(string errorMsg)
        {
            Status = SearchResultStatus.Error;
            Msg = errorMsg;
        }

        public SearchResult(BaseGeoLocation location, Stopwatch sw)
        {
            Status = location == null ? SearchResultStatus.NotFound : SearchResultStatus.Success;
            Locations = new List<BaseGeoLocation> { location };
            TimeMs = sw?.Elapsed.TotalMilliseconds ?? 0;
        }
    }
}
