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
        public Stream Stream
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
                    var stream = GetStream();
                    header = new BaseHeader(stream);
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
    }
}
