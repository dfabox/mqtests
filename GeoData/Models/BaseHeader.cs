using System;
using GeoData.Common;
using static GeoData.Common.BaseUtils;

namespace GeoData.Models
{
    /// <summary>
    /// Заголовок файла геоданных
    /// </summary>
    public class BaseHeader
    {
        public const uint SIZE = 60;

        public int Version { get; private set; }           // 0 4 версия база данных
        public string Name { get; private set; }           // 4 32 название/префикс для базы данных
        public ulong Timestamp { get; private set; }       // 36 8 время создания базы данных
        public int Records { get; private set; }           // 44 4 общее количество записей
        public uint OffsetRanges { get; private set; }     // 48 4 смещение относительно начала файла до начала списка записей с геоинформацией
        public uint OffsetCities { get; private set; }     // 52 4 смещение относительно начала файла до начала индекса с сортировкой по названию городов
        public uint OffsetLocations { get; private set; }  // 56 4 смещение относительно начала файла до начала списка записей о местоположении

        public BaseHeader(byte[] buffer)
        {
            if (buffer == null || buffer.Length < SIZE)
                throw new InvalidBufferException("BaseHeader", buffer?.Length ?? 0, SIZE);

            Version = BitConverter.ToInt32(buffer, 0);
            Name = buffer.GetStringFromBytes(4, 32);
            Timestamp = BitConverter.ToUInt64(buffer, 36);
            Records = BitConverter.ToInt32(buffer, 44);
            OffsetRanges = BitConverter.ToUInt32(buffer, 48);
            OffsetCities = BitConverter.ToUInt32(buffer, 52);
            OffsetLocations = BitConverter.ToUInt32(buffer, 56);
        }
    }
}
