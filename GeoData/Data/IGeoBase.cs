using GeoData.Models;

namespace GeoData.Data
{
    /// <summary>
    /// Интерфейс работы с базой местоположений
    /// </summary>
    public interface IGeoBase
    {
        /// <summary>
        /// Заголовок базы
        /// </summary>
        public BaseHeader Header { get; }

        /// <summary>
        /// Поиск метоположения по ip
        /// </summary>
        /// <param name="ip">значение ip</param>
        /// <returns></returns>
        public SearchResult FindLocationByIp(uint ip);

        /// <summary>
        /// Поиск метоположений по городу (может быть несколько)
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public SearchResult FindLocationByCity(string city);

        #region Методы получения данных из блоков файла - для отладки/тестирования
        public BaseGeoLocation GetLocationAt(uint index);
        public BaseIpRange GetIpRangeAt(uint index);
        #endregion
    }
}
