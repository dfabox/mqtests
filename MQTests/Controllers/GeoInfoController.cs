using GeoData.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MQTests.Controllers
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
            //var position = geoSearch?.GeoPositionFromIp(ip);
            var result = new SearchResult($"Запрос данных по IP не реализован {text}");

            return JsonConvert.SerializeObject(result);
        }

        [HttpGet]
        [Route("~/city/locations")]
        public string GetCityLocation(string text)
        {
            //var position = geoSearch?.GeoPositionFromCity(city);
            var result = new SearchResult($"Запрос данных по городу не реализован {text}");

            return JsonConvert.SerializeObject(result);
        }
    }
}
