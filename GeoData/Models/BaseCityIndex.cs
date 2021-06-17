using System;
using GeoData.Common;

namespace GeoData.Models
{
    /// <summary>
    /// Индекс записей городов
    /// </summary>
    public class BaseCityIndex
    {
        public const uint SIZE = 4;

        public uint LocationIndex; // адрес записи

        public BaseCityIndex(byte[] buffer)
        {
            if (buffer == null || buffer.Length < SIZE)
                throw new InvalidBufferException(nameof(BaseCityIndex), buffer?.Length ?? 0, SIZE);

            LocationIndex = BitConverter.ToUInt32(buffer, 0);
        }
    }
}
