using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WeatherReport.Web.Models;

namespace WeatherReport.Web.Controllers
{
    [RoutePrefix("api/temperatures")]
    public class TemperatureController : ApiController
    {
        ITemperatureService _temperatureService;

        public TemperatureController(ITemperatureService temperatureService)
        {
            _temperatureService = temperatureService;
        }

        [Route("average")]
        [HttpGet]
        public async Task<TemperatureData> GetAsync()
        {
            return await _temperatureService.GetAverageTemperatureForPastHourAsync(); ;
        }
    }
}
