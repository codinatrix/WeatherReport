using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherReport.Web.Models
{
    public class Precipitation
    {
        public double RainfallInMm { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}