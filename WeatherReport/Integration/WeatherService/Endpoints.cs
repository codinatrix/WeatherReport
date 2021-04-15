using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherReport.Integration.WeatherService
{
    public static class Endpoints
    {

        public const string AverageTemperature = "https://localhost:44305/api/temperature/average";
        public const string LundPrecipitation = "https://localhost:44305/api/precipitation/total";
        public const string AllTemperatures = "https://localhost:44305/api/temperature/all";
    }
}
