using System;
using System.IO;
using static GeoData.Base.BaseConsts;

namespace GeoData.Data
{
    public class GeoLocalBufferedBase : GeoStreamBase
    {
        public GeoLocalBufferedBase()
        {
            var baseFileName = GetLocalBaseFileName();
            var info = new FileInfo(baseFileName);

            var fileStream = new FileStream(baseFileName, FileMode.Open, FileAccess.Read);
            var stream = new BufferedStream(fileStream, Convert.ToInt32(Math.Min(int.MaxValue, info.Length)));

            SetStream(stream);
        }
    }
}
