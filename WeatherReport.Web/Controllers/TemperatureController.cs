using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WeatherReport.Web.Models;
using WeatherReport.Web.Services;

namespace WeatherReport.Web.Controllers
{
    [RoutePrefix("api/temperature")]
    public class TemperatureController : ApiController
    {
        ITemperatureService _temperatureService;

        public TemperatureController(ITemperatureService temperatureService)
        {
            _temperatureService = temperatureService;
        }

        [Route("average")]
        [HttpGet]
        public async Task<AverageTemperature> GetAverageTemperatureAsync()
        {
            return await _temperatureService.GetAverageTemperatureForPastHourAsync(); ;
        }

        [Route("all")]
        [HttpGet]
        public async Task<AllStationsTemperature> GetAllTemperaturesAsynx()
        {
            return await _temperatureService.GetTemperatureForEachStationAsync(); ;
        }

        
    }
}
