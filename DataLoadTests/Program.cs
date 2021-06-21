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
        private delegate bool Test1(IGeoBase geoBase, IGeoSearch geoSearch, uint index);

        // Реализация тестового поиска по городу
        private static bool TestCity1(IGeoBase geoBase, IGeoSearch geoSearch, uint index)
        {
            var location = geoBase.GetLocationAt(index);
            var city = location.City; // "cit_Ula"

            // Внесем долю ненайденных
            if (index.ToString().EndsWith("0"))
                city += "?";

            var result = geoSearch.FindLocationByCity(city);

            return result.Status == SearchResultStatus.Success;
        }

        // Реализация тестового поиска по ip
        private static bool TestIp1(IGeoBase geoBase, IGeoSearch geoSearch, uint index)
        {
            var ipRange = geoBase.GetIpRangeAt(index);
            var result = geoSearch.FindLocationByIp(ipRange.IpFrom + 1);

            return result.Status == SearchResultStatus.Success;
        }

        private static void DoTestFor(IGeoBase geoBase, IGeoSearch geoSearch, int testCount, Test1 test, int maxCount, string name)
        {
            var random = new Random();
            var sw = Stopwatch.StartNew();

            var successCount = 0;
            for (var i = 0; i < testCount; i++)
            {
                var index = random.Next(0, maxCount - 1);

                if (test(geoBase, geoSearch, Convert.ToUInt32(index)))
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
            var geoSearch = new GeoSearch(geoBase);
            var h = geoBase.Header;

            //for (var i = 0; i < 30; i++)
            //{
            //    var ip1 = geoBase.GetIpRangeAt(Convert.ToUInt32(geoBase.Header.Records - 100 + i));
            //    Console.WriteLine($"i1: {ip1.IpFrom}, i2: {ip1.IpTo}, ir: {ip1.LocationIndex}, i: {i}");
            //}

            //var ip1 = geoBase.GetIpRangeAt(Convert.ToUInt32(geoBase.Header.IpRangeRecords + 1));
            //var b1 = geoBase.ReadBuffer(geoBase.Header.OffsetRanges + Convert.ToUInt32(geoBase.Header.IpRangeRecords* BaseIpRange.SIZE), 100);
            //var s1 = BaseUtils.GetStringFromBytes(b1, 0, 8);
            //var s2 = BaseUtils.GetStringFromBytes(b1, 8, 20);

            w1.Stop();

            Console.WriteLine($"{typeof(T).Name} => время открытия: {w1.ElapsedMilliseconds}, имя: {h.Name}, версия: {h.Version}, кол.записей: {h.Records}");

            if (testIp)
                DoTestFor(geoBase, geoSearch, testCount, TestIp1, geoBase.Header.Records, "по ip");

            if (testCity)
                DoTestFor(geoBase, geoSearch, testCount, TestCity1, geoBase.Header.Records, "по городу");
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

            // TestGeoBase<GeoResourceBase>(testIp, testCity, testCount);
            // TestGeoBase<GeoLocalBase>(testIp, testCity, testCount);
            // TestGeoBase<GeoMappedBase>(testIp, testCity, testCount);
            // TestGeoBase<GeoLocalBufferedBase>(testIp, testCity, testCount);
            // TestGeoBase<GeoMemoryBase>(testIp, testCity, testCount);
            TestGeoBase<GeoResourceMemoryBase>(testIp, testCity, testCount);

            sw.Stop();
            //var b1 = allocBuffer[0];
            //Console.WriteLine(b1);
            Console.WriteLine($"общее время: {sw.ElapsedMilliseconds}");

            Console.ReadKey();
        }
    }
}
