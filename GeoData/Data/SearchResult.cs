using System.Collections.Generic;
using System.Diagnostics;
using GeoData.Models;

namespace GeoData.Data
{
    /// <summary>
    /// Результат поиска данных
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// Статус поиска
        /// </summary>
        public SearchResultStatus Status { get; private set; }

        /// <summary>
        /// Сообщение об ошибке (TODO некрасиво Exception показывать пользователю)
        /// </summary>
        public string Msg { get; private set; }

        /// <summary>
        /// Список местоположений по запросу
        /// </summary>
        public ICollection<BaseGeoLocation> Locations { get; private set; }

        /// <summary>
        /// Время выполнения поискового запроса
        /// </summary>
        public double TimeMs { get; private set; }

        /// <summary>
        /// Найденный диапазон ip, если поиск по ip
        /// </summary>
        public BaseIpRange IpRange { get; set; }

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
