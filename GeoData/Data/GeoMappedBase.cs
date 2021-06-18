using System.IO;
using System.IO.MemoryMappedFiles;
using static GeoData.Base.BaseConsts;

namespace GeoData.Data
{
    /// <summary>
    /// Реализация файла данных на основе отображаемого файла
    /// </summary>
    public class GeoMappedBase : GeoBase
    {
        private MemoryMappedFile mmFile;

        public GeoMappedBase()
        {
            var baseFileName = GetLocalBaseFileName();

            mmFile = MemoryMappedFile.CreateFromFile(baseFileName, FileMode.Open, null, 0, MemoryMappedFileAccess.Read);

            file = mmFile;

            LoadHeader();
        }

        protected override byte[] ReadBuffer(uint offset, uint size)
        {
            if (mmFile == null)
                return null;

            // TODO Реализовать проверку параметров для чтения буфера
            var va = mmFile.CreateViewStream(offset, size, MemoryMappedFileAccess.Read);
            var buffer = new byte[size];

            va.Read(buffer, 0, buffer.Length);

            return buffer;
        }
    }
}
