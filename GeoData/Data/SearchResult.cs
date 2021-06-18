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
        public int LocationCount => Locations?.Count ?? 0;
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

        public SearchResult(ICollection<BaseGeoLocation> locations, Stopwatch sw)
        {
            Status = locations == null && locations.Count > 0 ? SearchResultStatus.NotFound : SearchResultStatus.Success;
            Locations = locations;
            TimeMs = sw?.Elapsed.TotalMilliseconds ?? 0;
        }
    }
}
