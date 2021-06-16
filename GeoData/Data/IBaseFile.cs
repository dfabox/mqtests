using System.IO;
using GeoData.Models;

namespace GeoData.Data
{
    public interface IGeoFile
    {
        public Stream Stream { get; }
        public BaseHeader Header { get; }
    }
}
