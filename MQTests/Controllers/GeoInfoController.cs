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
        private readonly ILogger<GeoInfoController> logger;
        private readonly IGeoSearch geoSearch;

        public GeoInfoController(ILogger<GeoInfoController> logger, IGeoSearch geoSearch)
        {
            this.logger = logger;
            this.geoSearch = geoSearch;
        }

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

        [HttpGet]
        [Route("~/city/locations")]
        public string GetCityLocation(string text)
        {
            var result = geoSearch.FindLocationByCity(text);

            return JsonConvert.SerializeObject(result);
        }

        [HttpGet]
        [Route("~/test/rndip")]
        public string GetRandomIp(int count = 10)
        {
            // Список ip для тестового поиска
            var result = new TestResult(geoSearch.GetRandomIp(count));

            return JsonConvert.SerializeObject(result);
        }

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
