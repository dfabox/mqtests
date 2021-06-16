using System;
namespace GeoData.Models
{
    /// <summary>
    /// Местоположение
    /// </summary>
    public class BaseGeoPosition
    {
        //public sbyte country[8];        // название страны (случайная строка с префиксом "cou_")
        //public sbyte region[12];        // название области (случайная строка с префиксом "reg_")
        //public sbyte postal[12];        // почтовый индекс (случайная строка с префиксом "pos_")
        //public sbyte city[24];          // название города (случайная строка с префиксом "cit_")
        //public sbyte organization[32];  // название организации (случайная строка с префиксом "org_")
        public float latitude;                // широта
        public float longitude;               // долгота
    }
}
