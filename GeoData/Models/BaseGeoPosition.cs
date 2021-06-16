using System;
using GeoData.Common;
using static GeoData.Common.BaseUtils;

namespace GeoData.Models
{
    /// <summary>
    /// Местоположение
    /// </summary>
    public class BaseGeoPosition
    {
        private const int SIZE = 96;

        public string Country { get; private set; }       // 0 8 название страны (случайная строка с префиксом "cou_")
        public string Region { get; private set; }        // 8 12 название области (случайная строка с префиксом "reg_")
        public string Postal { get; private set; }        // 20 12 почтовый индекс (случайная строка с префиксом "pos_")
        public string City { get; private set; }          // 32 24 название города (случайная строка с префиксом "cit_")
        public string Organization { get; private set; }  // 56 32 название организации (случайная строка с префиксом "org_")
        public float Latitude { get; private set; }       // 88 4 широта
        public float Longitude { get; private set; }      // 92 4 долгота

        public BaseGeoPosition(byte[] buffer)
        {
            if (buffer == null || buffer.Length < SIZE)
                throw new InvalidBufferException("BaseGeoPosition", buffer?.Length ?? 0, SIZE);

            Country = buffer.GetStringFromBytes(0, 8);
            Region = buffer.GetStringFromBytes(8, 12);
            Postal = buffer.GetStringFromBytes(20, 12);
            City = buffer.GetStringFromBytes(32, 24);
            Organization = buffer.GetStringFromBytes(56, 32);
            Latitude = BitConverter.ToSingle(buffer.GetSubBytes(88, 4));
            Longitude = BitConverter.ToSingle(buffer.GetSubBytes(92, 4));
        }
    }
}
