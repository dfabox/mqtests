using System;
using System.Threading.Tasks;
using GeoData.Data;

namespace DataLoadTests


{
    class Program
    {

        static void Main(string[] args)
        {
            DateTime t0;

            t0 = DateTime.Now;
            using var baseFile1 = new GeoLocalFile();
            var h1 = baseFile1.Header;
            var s0 = DateTime.Now - t0;

            t0 = DateTime.Now;
            using var geoBase = new GeoResourceFile();
            var h = geoBase.Header;
            var s1 = DateTime.Now - t0;

            Console.WriteLine($"r: {s1.Milliseconds}, f: {s0.Milliseconds}");

            Console.WriteLine($"n: {h.Name}, v: {h.Version}, r: {h.Records}");

            uint pos = 99999;
            var p1 = geoBase.GetLocationAt(pos);
            Console.WriteLine($"n: {p1.City}");

            //for (uint i = 0; i < 10; i++)
            //{
            //    var p1 = search.GetLocationAt(i);
            //    Console.WriteLine($"n: {p1.City}");
            //}

            //for (uint i = 0; i < 10; i++)
            //{
            //    var i1 = search.GetIpRangeAt(i);
            //    Console.WriteLine($"ip: {i1.IpFrom} - {i1.IpTo}, a: {i1.LocationIndex}");
            //}

            //for (uint i = 0; i < 10; i++)
            //{
            //    var i1 = search.GetCityIndexAt(i);
            //    Console.WriteLine($"a: {i1.LocationIndex}");
            //}

            var city = p1.City; // "cit_Ula";
            var l1 = geoBase.GeoLocationByCity(city);
            Console.WriteLine($"s: {l1.Status}, c: {l1.Location?.City}, t: {l1.TimeMS}, t1: {l1.TimeMS/ pos}");

            Console.ReadKey();
        }
    }
}
