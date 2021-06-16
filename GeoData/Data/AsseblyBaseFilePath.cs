using System.IO;
using GeoData.Base;

namespace GeoData.Data
{
    public class AsseblyBaseFilePath : IBaseFilePath
    {
        public string FilePath => GetFilePath();

        private string GetFilePath()
        {
            var path = System.Reflection.Assembly.GetExecutingAssembly().Location;

            var directory = Path.Combine(Path.Combine(Path.GetDirectoryName(path), BaseConsts.BASE_PATH), BaseConsts.FILE_NAME);

            return directory;
        }
    }
}
