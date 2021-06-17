using System;
using System.IO;
using System.Linq;
using System.Reflection;
using GeoData.Base;

namespace GeoData.Data
{
    public class GeoResourceFile : GeoFile
    {
        private static Stream GetEmbeddedResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            if (assembly == null)
                return null;

            var names = assembly.GetManifestResourceNames();
            var name = names.FirstOrDefault(o => o.ToLower().EndsWith(resourceName.ToLower()));

            if (name == null)
                return null;

            return assembly.GetManifestResourceStream(name);
        }

        protected override Stream GetStream()
        {
            return GetEmbeddedResource($"{BaseConsts.BASE_PATH}.{BaseConsts.FILE_NAME}");
        }
    }
}
