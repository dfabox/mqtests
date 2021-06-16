using System;
namespace GeoData.Models
{
    /// <summary>
    /// Диапазон ip-адресов
    /// </summary>
    public struct BaseIpRange
    {
        public uint ip_from;           // начало диапазона IP адресов
        public uint ip_to;             // конец диапазона IP адресов
        public uint location_index;    // индекс записи о местоположении
    }
}
