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

        private static bool TestCity1(IGeoBase geoBase, uint pos)
        {
            var location = geoBase.GetLocationAt(pos);

            var city = location.City; // "cit_Ula";

            var result = geoBase.FindLocationByCity(city);

            return result.Status == SearchResultStatus.Success;
        }

        private static void TestCity(IGeoBase geoBase)
        {
            var random = new Random();
            var sw = Stopwatch.StartNew();

            var successCount = 0;
            for (var i = 0; i < TEST_COUNT; i++)
            {
                var pos = Convert.ToUInt32(random.Next(geoBase.Header.Records));
                if (TestCity1(geoBase, pos))
                    successCount += 1;
            }

            sw.Stop();
            var ms = sw.Elapsed.TotalMilliseconds;

            Console.WriteLine($"t: {ms}, success: {successCount}");
        }

        private static bool TestIp1(IGeoBase geoBase, int index)
        {
            var ipRange = geoBase.GetIpRangeAt(Convert.ToUInt32(index));

            var result = geoBase.FindLocationByIp(ipRange.IpFrom + 1);

            return result.Status == SearchResultStatus.Success;
        }

        private static void TestIp(IGeoBase geoBase)
        {
            var random = new Random();
            var sw = Stopwatch.StartNew();

            var successCount = 0;
            for (var i = 0; i < TEST_COUNT; i++)
            {
                var index = random.Next(0, IP_RANGE_COUNT - 1);

                if (TestIp1(geoBase, index))
                    successCount += 1;
            }

            sw.Stop();
            var ms = sw.Elapsed.TotalMilliseconds;

            Console.WriteLine($"t: {ms}, success: {successCount}");
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

            //for (uint i = 200; i < 350; i++)
            //{
            //    var address = geoBase.GetCityAddressAt(i);
            //    var city = geoBase.GetCityFromAddress(address);
            //    var location = geoBase.GetLocationFromAddress(address);

            //    Console.WriteLine($"a: {address}, c: {location.City}, p: {location.Postal}, o: {location.Organization}");
            //}

            w1.Stop();

            Console.WriteLine($"{typeof(T).Name} => t: {w1.ElapsedMilliseconds}, n: {h.Name}, v: {h.Version}, r: {h.Records}");

            //TestCity(geoBase);
            //TestIp(geoBase);
        }

        static void Main(string[] args)
        {
            // Выделить размер памяти для загрузки файла
            var baseFileName = GetLocalBaseFileName();
            var info = new FileInfo(baseFileName);
            var allocBuffer = new byte[info.Length* 3];

            //TestGeoBase<GeoLocalBase>();
            TestGeoBase<GeoResourceBase>();
            //TestGeoBase<GeoMappedBase>();

            var b1 = allocBuffer[0];
            Console.WriteLine(b1);
            Console.ReadKey();
        }
    }
}
