using System;
using System.Diagnostics;
using static GeoData.Base.BaseConsts;
using GeoData.Data;
using System.IO;

namespace DataLoadTests


{
    class Program
    {
        private static void TestCity(IGeoFile geoBase)
        {
            uint pos = 99999; // 99999;
            var p1 = geoBase.GetLocationAt(pos);
            Console.WriteLine($"n: {p1.City}");

            var city = p1.City; // "cit_Ula";
            var l1 = geoBase.FindLocationByCity(city);
            Console.WriteLine($"s: {l1.Status}, c: {l1.Location?.City}, t: {l1.TimeMS}, t1: {l1.TimeMS / pos}");
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

            Console.WriteLine($"t: {w1.ElapsedMilliseconds}, n: {h.Name}, v: {h.Version}, r: {h.Records}");

            //TestCity(geoBase);
        }

        static void Main(string[] args)
        {
            // Выделить размер памяти для загрузки файла
            //var baseFileName = GetLocalBaseFileName();
            //var info = new FileInfo(baseFileName);
            //var allocBuffer = new byte[info.Length];

            TestGeoBase<GeoLocalFile>();
            TestGeoBase<GeoResourceFile>();
            TestGeoBase<GeoMappedFile>();

            //allocBuffer = null;
            Console.ReadKey();
        }
    }
}
