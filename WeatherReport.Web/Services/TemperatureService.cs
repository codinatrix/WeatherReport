using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using WeatherReport.Web.Integration.SMHI;
using WeatherReport.Web.Models;

namespace WeatherReport.Web
{
    public interface ITemperatureService
    {
        Task<TemperatureData> GetAverageTemperatureForPastHourAsync();
    }

    public class TemperatureService : ITemperatureService
    {
        ISMHIGateway _sMHIGateway;

        public TemperatureService(ISMHIGateway sMHIGateway)
        {
            _sMHIGateway = sMHIGateway;
        }

        public async Task<TemperatureData> GetAverageTemperatureForPastHourAsync()
        {
            AverageTemperatureResponse temperatureReponse = await _sMHIGateway.GetTemperatureDataForThePastHourAsync();

            // We discard data for stations that are null
            // If SMHI has returned multiple values for a station, we grab the first one.
            // Unclear what it would mean if SMHI returned more than one value
            List<Value> nonNullValueObjsForEachStation = temperatureReponse.station
                .Where(s => s.value != null)
                .Select(s => s.value.FirstOrDefault()).ToList();

            // List contains one temperature for each station that had data
            List<string> temperatureForEachStation =  nonNullValueObjsForEachStation
                .Where(v => !String.IsNullOrWhiteSpace(v.value))
                .Select(v => v.value).ToList();

            List<double> allTemperaturesForSweden = new List<double>();

            // Temperature is received as a string, so we can convert it to double
            // We discard data that fails to convert
            foreach (string temperatureAsStr in temperatureForEachStation)
            {
                double temperatureAsDouble;
                if (double.TryParse(temperatureAsStr, out temperatureAsDouble))
                {
                    allTemperaturesForSweden.Add(temperatureAsDouble);
                }
            }

            if (!allTemperaturesForSweden.Any())
                throw new ExternalException("SMHI did not return any data.");

            TemperatureData temperatureData = new TemperatureData()
            {
                AverageTemperature = allTemperaturesForSweden.Average()
            };

            return temperatureData;
        }
    }
}