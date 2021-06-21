using GeoData.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MQGeoSearch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeoInfoController : ControllerBase
    {
        private readonly ILogger<GeoInfoController> logger;  // логгер
        private readonly IGeoSearch geoSearch;               // поиск местоположения

        public GeoInfoController(ILogger<GeoInfoController> logger, IGeoSearch geoSearch)
        {
            this.logger = logger;
            this.geoSearch = geoSearch;
        }

        // Запрос поиска местоположения по ip
        //  метод:    GET
        //  вызов:    /ip/location?text=123.123
        //  возврат:  строка в формате json с результатом поиска
        [HttpGet]
        [Route("~/ip/location")]
        public string GetIpLocation(string text)
        {
            var ipText = text?.Replace(".", "");

            SearchResult result;
            if (uint.TryParse(ipText, out var ipValue))
                result = geoSearch.FindLocationByIp(ipValue);
            else
                result = new SearchResult($"Некорректное значение ip-адреса {text}");

            return JsonConvert.SerializeObject(result);
        }

        // Запрос поиска местоположения по городу
        //  метод:    GET
        //  вызов:    /city/locations?text=cit_Usd
        //  возврат:  строка в формате json с результатом поиска
        [HttpGet]
        [Route("~/city/locations")]
        public string GetCityLocation(string text)
        {
            var result = geoSearch.FindLocationByCity(text);

            return JsonConvert.SerializeObject(result);
        }

        // Запрос списка ip-адресов для тестового поиска
        //  метод:    GET
        //  вызов:    /test/rndip?count=1000
        //  возврат:  строка в формате json со списком значений
        [HttpGet]
        [Route("~/test/rndip")]
        public string GetRandomIp(int count = 10)
        {
            // Список ip для тестового поиска
            var result = new TestResult(geoSearch.GetRandomIp(count));

            return JsonConvert.SerializeObject(result);
        }

        // Запрос списка названий городов для тестового поиска
        //  метод:    GET
        //  вызов:    /test/rndip?count=1000
        //  возврат:  строка в формате json со списком значений
        [HttpGet]
        [Route("~/test/rndcity")]
        public string GetRandomCity(int count = 10)
        {
            // Список городов для тестового поиска
            var result = new TestResult(geoSearch.GetRandomCity(count));

            return JsonConvert.SerializeObject(result);            
        }
    }
}
