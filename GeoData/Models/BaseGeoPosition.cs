﻿using System;
namespace GeoData.Struncts
{
    /// <summary>
    /// Местоположение
    /// </summary>
    public unsafe struct BaseGeoPosition
    {
        public fixed sbyte country[8];        // название страны (случайная строка с префиксом "cou_")
        public fixed sbyte region[12];        // название области (случайная строка с префиксом "reg_")
        public fixed sbyte postal[12];        // почтовый индекс (случайная строка с префиксом "pos_")
        public fixed sbyte city[24];          // название города (случайная строка с префиксом "cit_")
        public fixed sbyte organization[32];  // название организации (случайная строка с префиксом "org_")
        public float latitude;                // широта
        public float longitude;               // долгота
    }
}
