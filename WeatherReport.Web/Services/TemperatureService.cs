using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using WeatherReport.Web.Integration.SMHI;
using WeatherReport.Web.Models;

namespace WeatherReport.Web.Services
{
    public interface ITemperatureService
    {
        Task<AverageTemperature> GetAverageTemperatureForPastHourAsync();
        Task<AllStationsTemperature> GetTemperatureForEachStationAsync();

    }

    public class TemperatureService : ITemperatureService
    {
        ISMHIGateway _sMHIGateway;

        public TemperatureService(ISMHIGateway sMHIGateway)
        {
            _sMHIGateway = sMHIGateway;
        }

        public async Task<AverageTemperature> GetAverageTemperatureForPastHourAsync()
        {
            LastHourTemperature temperatureReponse = await _sMHIGateway.GetTemperatureDataForThePastHourAsync();

            // We discard data for stations that are null
            // If SMHI has returned multiple values for a station, we grab the first one.
            // Unclear what it would mean if SMHI returned more than one value
            List<LastHourTemperature.Value> nonNullValueObjsForEachStation = temperatureReponse.station
                .Where(s => s.value != null)
                .Select(s => s.value.FirstOrDefault()).ToList();

            // List contains one temperature for each station that had data
            List<string> temperatureForEachStation =  nonNullValueObjsForEachStation
                .Where(v => !string.IsNullOrWhiteSpace(v.value))
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
                throw new ExternalException("Failed to retrieve any data.");

            AverageTemperature temperatureData = new AverageTemperature()
            {
                Average = allTemperaturesForSweden.Average()
            };

            return temperatureData;
        }

        public async Task<AllStationsTemperature> GetTemperatureForEachStationAsync()
        {
            LastHourTemperature temperatureReponse = await _sMHIGateway.GetTemperatureDataForThePastHourAsync();

            AllStationsTemperature allStationsTemperature = new AllStationsTemperature()
            {
                Stations = new List<Station>()
            };

            foreach(LastHourTemperature.Station station in temperatureReponse.station)
            {
                
                List<LastHourTemperature.Value> stationValueList = station.value;

                // Discard null data
                if (stationValueList == null || !stationValueList.Any())
                    continue;

                string temperatureAsStr = stationValueList.First().value;

                double temperatureAsDouble;
                if (!double.TryParse(temperatureAsStr, out temperatureAsDouble))
                    continue;

                allStationsTemperature.Stations.Add(new Station()
                {
                    Name = station.name,
                    Temperature = temperatureAsDouble
                });
            }

            return allStationsTemperature;
        }
    }
}