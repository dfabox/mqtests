using System;
using System.IO;
using GeoData.Base;
using GeoData.Common;

namespace GeoData.Data
{
    /// <summary>
    /// Реализация копирования ресурса в поток в памяти
    /// </summary>
    public class GeoResourceMemoryBase : GeoStreamBase
    {
        public GeoResourceMemoryBase()
        {
            using var resource = BaseUtils.GetEmbeddedResource($"{BaseConsts.BASE_PATH}.{BaseConsts.FILE_NAME}");
            var stream = new MemoryStream(Convert.ToInt32(Math.Min(int.MaxValue, resource.Length)));
            resource.CopyTo(stream);

            SetStream(stream);
        }
    }
}
