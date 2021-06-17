using System;

namespace GeoData.Models
{
    public struct IpIndex
    {
        public uint IpFrom;             // 0 4 конец диапазона IP адресов
        public uint LocationIndex;    // 4 4 индекс записи о местоположении
    }
}
