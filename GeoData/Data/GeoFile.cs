using System;
using System.IO;
using GeoData.Models;

namespace GeoData.Data
{
    /// <summary>
    /// Абстрактная реализация файла базы
    /// </summary>
    public abstract class GeoFile : IGeoFile, IDisposable
    {
        private Stream stream;
        protected Stream Stream
        {
            get
            {
                if (stream == null)
                {
                    stream = GetStream();
                }

                return stream;
            }
        }

        private BaseHeader header;
        public BaseHeader Header
        {
            get
            {
                if (header == null)
                {
                    var buffer = ReadBuffer(0, BaseHeader.SIZE);
                    header = new BaseHeader(buffer);
                }

                return header;
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                if (stream != null)
                {
                    stream.Dispose();
                    stream = null;
                }
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract Stream GetStream();

        public byte[] ReadBuffer(uint offset, uint count)
        {
            // TODO Реализовать проверку параметр для чтения буфера

            var data = Stream;

            // TODO Реализовать безопасное чтение буфера из разных нитей
            lock (stream)
            {
                var reader = new BinaryReader(data);
                reader.BaseStream.Seek(offset, SeekOrigin.Begin);

                return reader.ReadBytes(Convert.ToInt32(count));
            }
        }
    }
}
