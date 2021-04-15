using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherReport.Cli.Integration.WeatherService
{
    public class PrecipitationResponse
    {
        public double RainfallInMm { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
