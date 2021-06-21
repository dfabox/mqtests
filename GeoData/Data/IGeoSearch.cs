using System.Collections.Generic;

namespace GeoData.Data
{
    /// <summary>
    /// Интерфейс поиска местоположения
    /// </summary>
    public interface IGeoSearch
    {
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

        /// <summary>
        /// Формирование списка ip для тестирования поиска
        /// </summary>
        /// <param name="count">количество ip</param>
        /// <returns></returns>
        public ICollection<string> GetRandomIp(int count);

        /// <summary>
        /// Формирование списка городов для тестирования поиска
        /// </summary>
        /// <param name="count">количество городов</param>
        /// <returns></returns>
        public ICollection<string> GetRandomCity(int count = 10);
    }
}
