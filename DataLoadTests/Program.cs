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
            var s1 = DateTime.Now - t0;

            t0 = DateTime.Now;
            using var baseFile2 = new GeoResourceFile();
            var h2 = baseFile1.Header;
            var s2 = DateTime.Now - t0;

            //using var header = MemoryMappedFile.CreateFromFile(basePath.FilePath, FileMode.Open, null, capacity: 0, access: MemoryMappedFileAccess.Read);

            //using var baseFile1 = MemoryMappedFile.CreateFromFile(basePath.FilePath, FileMode.Open, null, capacity: 0, access: MemoryMappedFileAccess.Read);
            //var s2 = DateTime.Now - t0;
            //using var mappedFile = MemoryMappedFile.CreateFromFile(fileStream: baseFile, null, 0, MemoryMappedFileAccess.Read, HandleInheritability.None, true);
            var s3 = DateTime.Now - t0;

            Console.WriteLine($"f: {s1}, r: {s2}, {s3}");

            Console.WriteLine($"n1: {h1.Name}, v1: {h1.Version}");
            Console.ReadKey();
        }
    }
}
