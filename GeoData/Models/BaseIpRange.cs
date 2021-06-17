using System;
using GeoData.Common;

namespace GeoData.Models
{
    /// <summary>
    /// Диапазон ip-адресов
    /// </summary>
    public class BaseIpRange
    {
        public const uint SIZE = 12;

        public uint IpFrom;           // 0 4 начало диапазона IP адресов
        public uint IpTo;             // 4 4 конец диапазона IP адресов
        public uint LocationIndex;    // 8 4 индекс записи о местоположении

        public BaseIpRange(byte[] buffer)
        {
            if (buffer == null || buffer.Length < SIZE)
                throw new InvalidBufferException(nameof(BaseIpRange), buffer?.Length ?? 0, SIZE);

            IpFrom = BitConverter.ToUInt32(buffer, 0);
            IpTo = BitConverter.ToUInt32(buffer, 4);
            LocationIndex = BitConverter.ToUInt32(buffer, 0);
        }
    }
}
