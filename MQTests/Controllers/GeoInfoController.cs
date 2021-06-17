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
        public string GetIpLocation(string ip)
        {
            var position = geoSearch?.GeoPositionFromIp(ip);

            return JsonConvert.SerializeObject(position);
        }

        [HttpGet]
        [Route("~/city/locations")]
        public string GetCityLocation(string city)
        {
            var position = geoSearch?.GeoPositionFromCity(city);

            return JsonConvert.SerializeObject(position);
        }
    }
}
