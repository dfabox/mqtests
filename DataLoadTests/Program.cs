using System;
using System.Diagnostics;
using static GeoData.Base.BaseConsts;
using GeoData.Data;
using System.IO;

namespace DataLoadTests
{
    class Program
    {
        private const int TEST_COUNT = 100000;

        private static void TestCity1(IGeoBase geoBase, uint pos)
        {
            var location = geoBase.GetLocationAt(pos);

            var city = location.City; // "cit_Ula";
            var location1 = geoBase.FindLocationByCity(city);
        }

        private static void TestCity(IGeoBase geoBase)
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

        private static bool TestIp1(IGeoBase geoBase, int index)
        {
            var ipRange = geoBase.GetIpRangeAt(Convert.ToUInt32(index));

            var location1 = geoBase.FindLocationByIp(ipRange.IpFrom + 1);

            return location1 != null;
        }

        private static void TestIp(IGeoBase geoBase)
        {
            var random = new Random();
            var w1 = Stopwatch.StartNew();

            var successCount = 0;
            for (var i = 0; i < TEST_COUNT; i++)
            {
                var index = random.Next(0, IP_RANGE_COUNT - 1);

                if (TestIp1(geoBase, index))
                    successCount += 1;
            }

            w1.Stop();
            Console.WriteLine($"t: {w1.Elapsed.TotalMilliseconds}, success: {successCount}");
        }

        public static T GetObject<T>(Type objType = null)
        {
            return (T)Activator.CreateInstance(objType ?? typeof(T));
        }

        private static void TestGeoBase<T>() where T : GeoBase
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
            TestGeoBase<GeoResourceBase>();
            //TestGeoBase<GeoMappedFile>();

            var b1 = allocBuffer[0];
            Console.WriteLine(b1);
            Console.ReadKey();
        }
    }
}
