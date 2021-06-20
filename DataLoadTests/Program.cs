using System;
using System.Diagnostics;
using static GeoData.Base.BaseConsts;
using GeoData.Data;
using System.IO;

namespace DataLoadTests
{
    class Program
    {
        // Делегат тестового поиска 
        private delegate bool Test1(IGeoBase geoBase, uint index);

        // Реализация тестового поиска по городу
        private static bool TestCity1(IGeoBase geoBase, uint index)
        {
            string city;
            // Внесем долю ненайденных
            if (index.ToString().EndsWith("0"))
                city = "cit_NotExists";
            else
            {
                var location = geoBase.GetLocationAt(index);
                city = location.City; // "cit_Ula"
            }

            var result = geoBase.FindLocationByCity(city);

            return result.Status == SearchResultStatus.Success;
        }

        // Реализация тестового поиска по ip
        private static bool TestIp1(IGeoBase geoBase, uint index)
        {
            var ipRange = geoBase.GetIpRangeAt(index);
            var result = geoBase.FindLocationByIp(ipRange.IpFrom + 1);

            return result.Status == SearchResultStatus.Success;
        }

        private static void DoTestFor(IGeoBase geoBase, int testCount, Test1 test, int maxCount, string name)
        {
            var random = new Random();
            var sw = Stopwatch.StartNew();

            var successCount = 0;
            for (var i = 0; i < testCount; i++)
            {
                var index = random.Next(0, maxCount - 1);

                if (test(geoBase, Convert.ToUInt32(index)))
                    successCount += 1;
            }

            sw.Stop();
            var ms = sw.Elapsed.TotalMilliseconds;
            var ms1 = Math.Round(ms / testCount, 3);
            var perSec = Math.Round(1000 * testCount / ms, 1);

            Console.WriteLine($"  {name} общее время: {ms} ms, успешно: {successCount}, на 1 поиск: {ms1} ms, в секунду: {perSec}");
        }

        public static T GetObject<T>(Type objType = null)
        {
            return (T)Activator.CreateInstance(objType ?? typeof(T));
        }

        private static void TestGeoBase<T>(bool testIp, bool testCity, int testCount) where T : GeoBase
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

            Console.WriteLine($"{typeof(T).Name} => время открытия: {w1.ElapsedMilliseconds}, имя: {h.Name}, версия: {h.Version}, кол.записей: {h.Records}");

            if (testIp)
                DoTestFor(geoBase, testCount, TestIp1, IP_RANGE_COUNT, "по ip");

            if (testCity)
                DoTestFor(geoBase, testCount, TestCity1, IP_RANGE_COUNT, "по городу");
        }

        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();

            var baseFileName = GetLocalBaseFileName();
            var info = new FileInfo(baseFileName);
            var allocBuffer = new byte[info.Length * 2];

            var testIp = true;
            var testCity = true;
            var testCount = 100000;

            //TestGeoBase<GeoResourceBase>(testIp, testCity, testCount);
            //TestGeoBase<GeoLocalBase>(testIp, testCity, testCount);
            //TestGeoBase<GeoMappedBase>(testIp, testCity, testCount);
            //TestGeoBase<GeoLocalBufferedBase>(testIp, testCity, testCount);
            TestGeoBase<GeoMemoryBase>(testIp, testCity, testCount); 

            sw.Stop();
            //var b1 = allocBuffer[0];
            //Console.WriteLine(b1);
            Console.WriteLine($"общее время: {sw.ElapsedMilliseconds}");

            Console.ReadKey();
        }
    }
}
