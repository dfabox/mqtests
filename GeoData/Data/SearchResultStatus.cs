namespace GeoData.Data
{
    public enum SearchResultStatus
    {
        None = 0,       // Неопределен
        Success = 1,    // Успешный поиск
        NotFound = 2,   // Данные по запросу не найдены
        Error = 3,      // Ошибка выполнения поиска 
    }
}
