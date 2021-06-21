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
        /// Местоположение по индексу
        /// </summary>
        /// <param name="index">индекс</param>
        /// <returns>местоположение по индексу</returns>
        public BaseGeoLocation GetLocationAt(uint index);

        /// <summary>
        /// Местоположение по адресу
        /// </summary>
        /// <param name="address">адрес местоположения в базе относительно начального смещения</param>
        /// <returns></returns>
        public BaseGeoLocation GetLocationFromAddress(uint address);

        /// <summary>
        /// Получение ip-диапазона по индексу
        /// </summary>
        /// <param name="index">индекс</param>
        /// <returns>диапазон по индексу</returns>
        public BaseIpRange GetIpRangeAt(uint index);

        /// <summary>
        /// Адрес города по индексу
        /// </summary>
        /// <param name="index">индекс</param>
        /// <returns>название города</returns>
        public uint GetCityAddressAt(uint index);

        /// <summary>
        /// Название города по пдресу
        /// </summary>
        /// <param name="address">адрес местоположения в базе относительно начального смещения</param>
        /// <returns>название города</returns>
        public string GetCityFromAddress(uint address);
    }
}
