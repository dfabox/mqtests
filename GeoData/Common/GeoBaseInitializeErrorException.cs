using System;
namespace GeoData.Common
{
    public class GeoBaseInitializeErrorException : Exception
    {
        public GeoBaseInitializeErrorException(string err)
            : base("Ошибка инициализации базы георасположений: " + err)
        {
        }
    }
}
