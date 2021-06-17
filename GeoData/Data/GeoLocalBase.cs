using System.IO;
using static GeoData.Base.BaseConsts;

namespace GeoData.Data
{
    public class GeoLocalBase : GeoStreamBase
    {
        public GeoLocalBase()
        {
            var baseFileName = GetLocalBaseFileName();
            var stream = new FileStream(baseFileName, FileMode.Open, FileAccess.Read);

            SetStream(stream);
        }
    }
}
