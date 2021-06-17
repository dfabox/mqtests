using GeoData.Data;
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
        private readonly IGeoBase geoFile;

        public GeoInfoController(ILogger<GeoInfoController> logger, IGeoBase geoFile)
        {
            this.logger = logger;
            this.geoFile = geoFile;
        }

        [HttpGet]
        [Route("~/ip/location")]
        public string GetIpLocation(string text)
        {
            var result = geoFile?.FindLocationByIp(text);
            //var result = new SearchResult($"Запрос данных по IP не реализован {text}");

            return JsonConvert.SerializeObject(result);
        }

        [HttpGet]
        [Route("~/city/locations")]
        public string GetCityLocation(string text)
        {
            var result = geoFile?.FindLocationByCity(text);
            //var result = new SearchResult($"Запрос данных по городу не реализован {text}");

            return JsonConvert.SerializeObject(result);
        }
    }
}
