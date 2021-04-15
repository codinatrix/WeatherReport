using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherReport.Web.Integration.SMHI
{
    public static class Endpoints
    {
        public const string AllTemperatureLatestHour = "https://opendata-download-metobs.smhi.se/api/version/1.0/parameter/1/station-set/all/period/latest-hour/data.json";
        public const string LundPrecipitationLatestMonth = "https://opendata-download-metobs.smhi.se/api/version/1.0/parameter/23/station/53430/period/latest-months/data.json";
    }
}