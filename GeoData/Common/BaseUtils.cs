using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

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

            return new string(chars).Trim().Trim('\0');
        }

        /// <summary>
        /// Преобразование строки в массив байтов с ограничением размера
        /// </summary>
        /// <param name="value">строка</param>
        /// <param name="size">размер массива</param>
        /// <param name="right">заполнитель</param>
        /// <returns>массив байт (символов)</returns>
        public static byte[] GetBytesFromStringLen(string value, int size, char right)
        {
            value ??= "";

            var result = Encoding.ASCII.GetBytes(new string(right, size));
            var len = Math.Min(size, value.Length);

            var chars = value.ToCharArray(0, len);
            for (var i = 0; i < len; i++)
                result[i] = (byte)chars[i];

            return result;
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

            // TODO Обработать ошибки размера буфера ?

            var bytes = new byte[lenght];
            Buffer.BlockCopy(buffer, offset, bytes, 0, lenght);

            return bytes;
        }

        /// <summary>
        /// Поток для ресурса сборки
        /// </summary>
        /// <param name="resourceName">имя ресурса</param>
        /// <returns></returns>
        public static Stream GetEmbeddedResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            if (assembly == null)
                return null;

            var names = assembly.GetManifestResourceNames();
            var name = names.FirstOrDefault(o => o.ToLower().EndsWith(resourceName.ToLower()));

            if (name == null)
                return null;

            return assembly.GetManifestResourceStream(name);
        }
    }
}
