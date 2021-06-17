using System;
namespace GeoData.Common
{
    public class InvalidBufferException : Exception
    {
        public InvalidBufferException(string Msg, int size, uint needSize)
            : base($"Некорректный буфер чтения данных: {Msg}, размер {size}, требуется {needSize}")
        {
        }
    }
}
