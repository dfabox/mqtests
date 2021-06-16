using System;
using System.IO;

namespace GeoData.Data
{
    public class AssemblyBaseFile : IBaseFileLoader
    {
        public Stream GetStream()
        {
            var basePath = new AsseblyBaseFilePath();

            var baseFile = new FileStream(basePath.FilePath, FileMode.Open, FileAccess.Read);

            return baseFile;
        }
    }
}
