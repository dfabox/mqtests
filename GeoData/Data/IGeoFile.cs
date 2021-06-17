using GeoData.Models;

namespace GeoData.Data
{
    public interface IGeoFile
    {
        public BaseHeader Header { get; }

        public byte[] ReadBuffer(uint offset, uint count);
    }
}
