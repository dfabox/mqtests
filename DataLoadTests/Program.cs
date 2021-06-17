using System;
using System.Threading.Tasks;
using GeoData.Data;
using GeoData.Search;

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
            using var baseFile = new GeoResourceFile();
            var h = baseFile.Header;
            var s1 = DateTime.Now - t0;

            Console.WriteLine($"r: {s1.Milliseconds}, f: {s0.Milliseconds}");

            Console.WriteLine($"n1: {h.Name}, v1: {h.Version}");

            var search = new GeoSearch(baseFile);
            var p1 = search.GetPositionAt(1);

            Console.WriteLine($"n: {p1.City}");

            Console.ReadKey();
        }
    }
}
