using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherReport.Web.Integration.SMHI
{
    public class AverageTemperatureResponse
    {
        public List<Station> station { get; set; }
        public long updated { get; set; }
        public Parameter parameter { get; set; }
        public Period period { get; set; }
        public List<Link> link { get; set; }
    }

    public class Value
    {
        public object date { get; set; }
        public string value { get; set; }
        public string quality { get; set; }
    }

    public class Station
    {
        public string key { get; set; }
        public string name { get; set; }
        public string owner { get; set; }
        public string ownerCategory { get; set; }
        public object from { get; set; }
        public object to { get; set; }
        public double height { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public List<Value> value { get; set; }
    }

    public class Parameter
    {
        public string key { get; set; }
        public string name { get; set; }
        public string summary { get; set; }
        public string unit { get; set; }
    }

    public class Period
    {
        public string key { get; set; }
        public long from { get; set; }
        public long to { get; set; }
        public string summary { get; set; }
        public string sampling { get; set; }
    }

    public class Link
    {
        public string rel { get; set; }
        public string type { get; set; }
        public string href { get; set; }
    }
}