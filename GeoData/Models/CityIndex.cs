using System;

namespace GeoData.Models
{
    public struct CityIndex
    {
        public byte[] CityBytes;      // байтовый массив названия городов
        public uint LocationIndex;    // 4 4 индекс записи о местоположении
    }
}
