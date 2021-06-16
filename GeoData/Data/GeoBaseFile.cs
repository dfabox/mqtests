using System;
using System.IO;

namespace GeoData.Data
{
    public class GeoBaseFile : IBaseFileLoader
    {
        public Stream GetStream()
        {
            var basePath = new GeoBaseFilePath();

            var baseFile = new FileStream(basePath.FilePath, FileMode.Open, FileAccess.Read);

            return baseFile;
        }
    }
}
