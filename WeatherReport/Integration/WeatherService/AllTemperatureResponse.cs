using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherReport.Cli.Integration.WeatherService
{
    public class AllTemperatureResponse
    {
        public List<StationResponse> Stations { get; set; }
    }
}
