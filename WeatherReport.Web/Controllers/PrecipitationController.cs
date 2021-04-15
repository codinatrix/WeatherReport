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
    [RoutePrefix("api/precipitation")]

    public class PrecipitationController : ApiController
    {
        IPrecipitationService _precipitationService;

        public PrecipitationController(IPrecipitationService precipitationService)
        {
            _precipitationService = precipitationService;
        }

        [Route("total")]
        [HttpGet]
        public async Task<Precipitation> GetPrecipitationDataAsync()
        {
            return await _precipitationService.GetTotalPrecipitationForPastMonthsAsync(); ;
        }
    }
}