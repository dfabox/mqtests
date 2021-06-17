using System.IO;
using static GeoData.Base.BaseConsts;

namespace GeoData.Data
{
    public class GeoLocalFile : GeoStreamFile
    {
        public GeoLocalFile()
        {
            var baseFileName = GetLocalBaseFileName();
            var stream = new FileStream(baseFileName, FileMode.Open, FileAccess.Read);

            SetStream(stream);
        }
    }
}
