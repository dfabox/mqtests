using GeoData.Models;

namespace GeoData.Data
{
    public interface IGeoFile
    {
        public BaseHeader Header { get; }

        public byte[] ReadBuffer(int offset, int count);
    }
}
