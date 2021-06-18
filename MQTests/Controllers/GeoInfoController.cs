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
            var ipText = text?.Replace(".", "");

            SearchResult result;
            if (uint.TryParse(ipText, out var ipValue))
                result = geoFile?.FindLocationByIp(ipValue);
            else
                result = new SearchResult($"Некорректное значение ip-адреса {text}");

            return JsonConvert.SerializeObject(result);
        }

        [HttpGet]
        [Route("~/city/locations")]
        public string GetCityLocation(string text)
        {
            var result = geoFile?.FindLocationByCity(text);

            return JsonConvert.SerializeObject(result);
        }

        [HttpGet]
        [Route("~/test/rndip")]
        public string GetRandomIp()
        {
            return null;
        }

        [HttpGet]
        [Route("~/test/rndcity")]
        public string GetRandomCity()
        {
            var random = new Random();
            var index = Convert.ToUInt32(random.Next(geoBase.Header.Records));

            var successCount = 0;
            for (var i = 0; i < TEST_COUNT; i++)
            {

                if (TestCity1(geoBase, pos))
                    successCount += 1;
            }
        }
    }
}
