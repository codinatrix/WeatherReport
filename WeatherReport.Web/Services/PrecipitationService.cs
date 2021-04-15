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

    public interface IPrecipitationService
    {
        Task<Precipitation> GetTotalPrecipitationForPastMonthsAsync();

    }
    public class PrecipitationService : IPrecipitationService
    {
        ISMHIGateway _sMHIGateway;

        public PrecipitationService(ISMHIGateway sMHIGateway)
        {
            _sMHIGateway = sMHIGateway;
        }

        public async Task<Precipitation> GetTotalPrecipitationForPastMonthsAsync()
        {
            LatestMonthsPrecipitationResponse precipitationReponse = await _sMHIGateway.GetPrecipitationDataForLatestMonthsAsync();

            DateTime from = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(precipitationReponse.period.from);
            DateTime to = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(precipitationReponse.period.to);

            List<string> mmForEachStation = precipitationReponse.value
                .Where(s => s.value != null)
                .Select(s => s.value).ToList();

            List<double> allAmountsForStation = new List<double>();
            foreach (string mmAsStr in mmForEachStation)
            {
                double mmAsDouble;
                if (double.TryParse(mmAsStr, out mmAsDouble))
                {
                    allAmountsForStation.Add(mmAsDouble);
                }
            }

            if (!allAmountsForStation.Any())
                throw new ExternalException("Failed to retrieve any data.");


            Precipitation precipitationData = new Precipitation()
            {
                From = from,
                To = to,
                RainfallInMm = allAmountsForStation.Sum()
            };

            return precipitationData;
        }
    }
}