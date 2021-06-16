using System.IO;

namespace GeoData.Data
{
    public class GeoLocalFile : GeoFile
    {
        protected override Stream GetStream()
        {
            var basePath = new GeoBaseFilePath();

            var baseFile = new FileStream(basePath.FilePath, FileMode.Open, FileAccess.Read);

            return baseFile;
        }
    }
}
