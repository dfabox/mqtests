using System.IO;

namespace GeoData.Base
{
    public static class BaseConsts
    {
        public const string BASE_PATH = "base";
        public const string FILE_NAME = "geobase.dat";

        // Реальное количество диапазонов iЗ
        public const int IP_RANGE_COUNT = 12500;

        public static string GetLocalBaseFileName()
        {
            var path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var fileName = Path.Combine(Path.Combine(Path.GetDirectoryName(path), BASE_PATH), FILE_NAME);

            return fileName;
        }
    }
}
