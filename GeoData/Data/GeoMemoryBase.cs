using System;
using System.IO;
using static GeoData.Base.BaseConsts;

namespace GeoData.Data
{
    public class GeoMemoryBase : GeoStreamBase
    {
        public GeoMemoryBase()
        {
            var baseFileName = GetLocalBaseFileName();
            var info = new FileInfo(baseFileName);

            var fileStream = new FileStream(baseFileName, FileMode.Open, FileAccess.Read);
            var stream = new MemoryStream(Convert.ToInt32(Math.Min(int.MaxValue, info.Length)));
            fileStream.CopyTo(stream);

            SetStream(stream);
        }
    }
}
