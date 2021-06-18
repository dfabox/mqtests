using System;
using System.IO;

namespace GeoData.Data
{
    /// <summary>
    /// Адбстракция файла данных на основе потока
    /// </summary>
    public abstract class GeoStreamBase : GeoBase
    {
        private Stream stream;

        protected void SetStream(Stream stream)
        {
            this.stream = stream;
            file = stream;

            LoadHeader();
        }

        protected override byte[] ReadBuffer(uint offset, uint count)
        {
            // TODO Реализовать проверку параметров для чтения буфера

            var reader = new BinaryReader(stream);
            byte[] result = null;

            lock (stream)
            {
                reader.BaseStream.Seek(offset, SeekOrigin.Begin);
                result = reader.ReadBytes(Convert.ToInt32(count));
            }

            return result;
        }
    }
}
