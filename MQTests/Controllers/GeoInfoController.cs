using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MQTests.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeoInfoController : ControllerBase
    {
        private readonly ILogger<GeoInfoController> _logger;

        public GeoInfoController(ILogger<GeoInfoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("~/ip/location")]
        public string GetIpLocation(string ip)
        {
            return null;
        }

        [HttpGet]
        [Route("~/city/locations")]
        public string GetCityLocation(string city)
        {
            return null;
        }
    }
}
