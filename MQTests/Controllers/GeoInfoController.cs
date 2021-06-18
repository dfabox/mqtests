using System;
using GeoData.Data;
using static GeoData.Base.BaseConsts;
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
        private readonly IGeoBase geoBase;

        public GeoInfoController(ILogger<GeoInfoController> logger, IGeoBase geoBase)
        {
            this.logger = logger;
            this.geoBase = geoBase;
        }

        [HttpGet]
        [Route("~/ip/location")]
        public string GetIpLocation(string text)
        {
            var ipText = text?.Replace(".", "");

            SearchResult result;
            if (uint.TryParse(ipText, out var ipValue))
                result = geoBase.FindLocationByIp(ipValue);
            else
                result = new SearchResult($"Некорректное значение ip-адреса {text}");

            return JsonConvert.SerializeObject(result);
        }

        [HttpGet]
        [Route("~/city/locations")]
        public string GetCityLocation(string text)
        {
            var result = geoBase.FindLocationByCity(text);

            return JsonConvert.SerializeObject(result);
        }

        [HttpGet]
        [Route("~/test/rndip")]
        public string GetRandomIp()
        {
            var random = new Random();
            var index = Convert.ToUInt32(random.Next(geoBase.Header.Records));
            var ipRange = geoBase.GetIpRangeAt(index);

            return (ipRange.IpFrom + 1).ToString();
        }

        [HttpGet]
        [Route("~/test/rndcity")]
        public string GetRandomCity()
        {
            var random = new Random();
            var index = Convert.ToUInt32(random.Next(IP_RANGE_COUNT));
            var city = geoBase.GetLocationAt(index)?.City;

            return city;
            
        }
    }
}
