using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace GeoData.Common
{
    public static class BaseUtils
    {
        /// <summary>
        /// Structure is converted to a byte array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] StructToBytes<T>(T obj)
        {
            int size = Marshal.SizeOf(typeof(T));
            IntPtr bufferPtr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(obj, bufferPtr, false);
                byte[] bytes = new byte[size];
                Marshal.Copy(bufferPtr, bytes, 0, size);
                return bytes;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in StructToBytes ! " + ex.Message);
            }
            finally
            {
                Marshal.FreeHGlobal(bufferPtr);
            }
        }

        /// <summary>
        /// byte array is converted to structure
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object BytesToStuct(byte[] bytes, Type type)
        {
            // Get the size of the structure
            int size = Marshal.SizeOf(type);
            //byte array length is less than the size of the structure
            if (size > bytes.Length)
            {
                // Return empty
                return null;
            }
            // allocate the size of the memory space
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            // Copy the byte array to the allocated memory space
            Marshal.Copy(bytes, 0, structPtr, size);
            // Convert the memory space to the target structure
            object obj = Marshal.PtrToStructure(structPtr, type);
            // Free up the memory space
            Marshal.FreeHGlobal(structPtr);
            // Return structure
            return obj;
        }

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
