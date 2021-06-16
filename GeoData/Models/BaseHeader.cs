using System;
using System.IO;
using System.Linq;

namespace GeoData.Models
{
    /// <summary>
    /// Заголовок файла геоданных
    /// </summary>
    public class BaseHeader
    {
        public int Version { get; private set; }           // версия база данных
        public sbyte[] Name { get; private set; }          // [32] название/префикс для базы данных
        public ulong Timestamp { get; private set; }       // время создания базы данных
        public int Records { get; private set; }           // общее количество записей
        public uint OffsetRanges { get; private set; }     // смещение относительно начала файла до начала списка записей с геоинформацией
        public uint OffsetCities { get; private set; }     // смещение относительно начала файла до начала индекса с сортировкой по названию городов
        public uint OffsetLocations { get; private set; }  // смещение относительно начала файла до начала списка записей о местоположении

        /// <summary>
        /// Строка названия/префикса
        /// </summary>
        public string NameText => new string(Name.Select(o => (char)o).ToArray())?.Trim();

        public BaseHeader(Stream stream)
        {
            using var reader = new BinaryReader(stream);

            Version = reader.ReadInt32();
            Name = reader.ReadBytes(32).Select(o => (sbyte)o).ToArray();
            Timestamp = reader.ReadUInt64();
            Records = reader.ReadInt32();
            OffsetRanges = reader.ReadUInt32();
            OffsetCities = reader.ReadUInt32();
            OffsetLocations = reader.ReadUInt32();
        }
    }
}
