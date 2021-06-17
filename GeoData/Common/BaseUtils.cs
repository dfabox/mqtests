using System;
using System.IO;
using System.Linq;

namespace GeoData.Common
{
    public static class BaseUtils
    {
        /// <summary>
        /// Преобразование массива байтов в строку
        /// </summary>
        /// <param name="bytes">массив байт для преобразования</param>
        /// <returns>строковое представление</returns>
        public static string GetStringFromBytes(this byte[] buffer, int offset, int lenght)
        {
            if (buffer == null || buffer.Length == 0)
                return null;

            // TODO Обработать ошибки размера буфера
            var bytes = buffer.GetSubBytes(offset, lenght);

            var chars = bytes.Select(o => (char)o).ToArray(); //new char[lenght];
            //Buffer.BlockCopy(bytes, 0, chars, 0, lenght);

            return new string(chars).Trim();
        }

        /// <summary>
        /// Получение подмассива байтов 
        /// </summary>
        /// <param name="buffer">исходный массив байтов</param>
        /// <param name="offset">смещение</param>
        /// <param name="lenght">длина</param>
        /// <returns></returns>
        public static byte[] GetSubBytes(this byte[] buffer, int offset, int lenght)
        {
            if (buffer == null)
                return null;

            // TODO Обработать ошибки размера буфера

            var bytes = new byte[lenght];
            Buffer.BlockCopy(buffer, offset, bytes, 0, lenght);

            return bytes;
        }
    }
}
