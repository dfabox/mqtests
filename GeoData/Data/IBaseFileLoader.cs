using System;
using System.IO;

namespace GeoData.Data
{
    public interface IBaseFileLoader
    {
        public Stream GetStream();
    }
}
