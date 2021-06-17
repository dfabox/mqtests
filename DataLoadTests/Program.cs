using System;
using System.Diagnostics;
using static GeoData.Base.BaseConsts;
using GeoData.Data;
using System.IO;

namespace DataLoadTests


{
    class Program
    {
        private const int TEST_COUNT = 10;

        private static void TestCity1(IGeoFile geoBase, uint pos)
        {
            var location = geoBase.GetLocationAt(pos);

            var city = location.City; // "cit_Ula";
            var location1 = geoBase.FindLocationByCity(city);
        }

        private static void TestCity(IGeoFile geoBase)
        {
            var random = new Random();
            var w1 = Stopwatch.StartNew();

            for (var i = 0; i < TEST_COUNT; i++)
            {
                var pos = Convert.ToUInt32(random.Next(geoBase.Header.Records));
                TestCity1(geoBase, pos);
            }

            w1.Stop();
            Console.WriteLine($"t: {w1.ElapsedMilliseconds}");
        }

        private static void TestIp1(IGeoFile geoBase, uint ip)
        {
            var location1 = geoBase.FindLocationByIp(ip.ToString());
        }

        private static void TestIp(IGeoFile geoBase)
        {
            var random = new Random();
            var w1 = Stopwatch.StartNew();

            var ipMin = geoBase.GetIpRangeAt(0).IpFrom;
            var ipMax = geoBase.GetIpRangeAt(Convert.ToUInt32(geoBase.Header.Records - 1)).IpTo;

            for (var i = 0; i < TEST_COUNT; i++)
            {
                var ip = Convert.ToUInt32(random.Next(geoBase.Header.Records));
                TestIp1(geoBase, ip);
            }

            w1.Stop();
            Console.WriteLine($"t: {w1.ElapsedMilliseconds}");
        }

        public static T GetObject<T>(Type objType = null)
        {
            return (T)Activator.CreateInstance(objType ?? typeof(T));
        }

        private static void TestGeoBase<T>() where T : GeoFile
        {
            var w1 = Stopwatch.StartNew();
            using var geoBase = GetObject<T>();
            var h = geoBase.Header;
            w1.Stop();

            Console.WriteLine($"{typeof(T).Name} => t: {w1.ElapsedMilliseconds}, n: {h.Name}, v: {h.Version}, r: {h.Records}");

            //TestCity(geoBase);
            TestIp(geoBase);
        }

        static void Main(string[] args)
        {
            // Выделить размер памяти для загрузки файла
            var baseFileName = GetLocalBaseFileName();
            var info = new FileInfo(baseFileName);
            var allocBuffer = new byte[info.Length* 3];

            //TestGeoBase<GeoLocalFile>();
            TestGeoBase<GeoResourceFile>();
            //TestGeoBase<GeoMappedFile>();

            var b1 = allocBuffer[0];
            Console.WriteLine(b1);
            Console.ReadKey();
        }
    }
}
