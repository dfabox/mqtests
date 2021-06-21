﻿using System.IO;

namespace GeoData.Base
{
    public static class BaseConsts
    {
        // Имя папки с локальным файлом или ресурсом
        public const string BASE_PATH = "base";

        // Имя файла данных и ресурса
        public const string FILE_NAME = "geobase.dat";

        // Имя локального файла данных
        public static string GetLocalBaseFileName()
        {
            var path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var fileName = Path.Combine(Path.Combine(Path.GetDirectoryName(path), BASE_PATH), FILE_NAME);

            return fileName;
        }
    }
}
