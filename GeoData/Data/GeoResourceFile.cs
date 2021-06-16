using System;
using System.IO;
using System.Linq;
using System.Reflection;
using GeoData.Base;

namespace GeoData.Data
{
    public class GeoResourceFile : IBaseFileLoader
    {
        private static Assembly GetAssembly(string AssemblyName = null)
        {
            if (string.IsNullOrWhiteSpace(AssemblyName))
                return Assembly.GetExecutingAssembly();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies(); //.OrderByDescending(o => o.FullName);

            return assemblies.FirstOrDefault(a => a.GetName().Name == AssemblyName);
        }

        private static Stream GetEmbeddedResource(string resourceName, string AssemblyName = null)
        {
            var assembly = GetAssembly(AssemblyName);

            if (assembly == null)
                return null;

            var names = assembly.GetManifestResourceNames();
            var name = names.FirstOrDefault(o => o.EndsWith(resourceName));

            if (name == null)
                return null;

            return assembly.GetManifestResourceStream(name);
        }

        public Stream GetStream()
        {
            return GetEmbeddedResource($"{BaseConsts.BASE_PATH}.{BaseConsts.FILE_NAME}");
        }
    }
}
