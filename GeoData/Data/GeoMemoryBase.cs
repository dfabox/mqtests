using System;
using System.IO;
using static GeoData.Base.BaseConsts;

namespace GeoData.Data
{
    /// <summary>
    /// Реализация копирования файла в поток в памяти
    /// </summary>
    public class GeoMemoryBase : GeoStreamBase
    {
        public GeoMemoryBase()
        {
            var baseFileName = GetLocalBaseFileName();

            using var fileStream = new FileStream(baseFileName, FileMode.Open, FileAccess.Read);
            var stream = new MemoryStream(Convert.ToInt32(Math.Min(int.MaxValue, fileStream.Length)));
            fileStream.CopyTo(stream);

            SetStream(stream);
        }
    }
}
