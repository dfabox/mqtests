using GeoData.Base;
using GeoData.Common;

namespace GeoData.Data
{
    /// <summary>
    /// Реализация чтения файла данных из ресурса сборки
    /// </summary>
    public class GeoResourceBase : GeoStreamBase
    {
        public GeoResourceBase()
        {
            var stream = BaseUtils.GetEmbeddedResource($"{BaseConsts.BASE_PATH}.{BaseConsts.FILE_NAME}");

            SetStream(stream);
        }
    }
}
