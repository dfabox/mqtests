using System;
using System.IO;

namespace GeoData.Data
{
    public class GeoStreamBase : GeoBase
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
            reader.BaseStream.Seek(offset, SeekOrigin.Begin);

            return reader.ReadBytes(Convert.ToInt32(count));
        }
    }
}
