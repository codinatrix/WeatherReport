using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;

namespace WeatherReport.Web.Integration.SMHI
{
	public interface ISMHIGateway {
		Task<LastHourTemperature> GetTemperatureDataForThePastHourAsync();
		Task<LatestMonthsPrecipitationResponse> GetPrecipitationDataForLatestMonthsAsync();
	}

	public class SMHIGateway : ISMHIGateway
	{

        public async Task<LastHourTemperature> GetTemperatureDataForThePastHourAsync()
        {

				using (var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(10) })
				using (HttpResponseMessage response =
					await httpClient.GetAsync(new Uri(Endpoints.AllTemperatureLatestHour)))
				{
					httpClient.DefaultRequestHeaders.Accept.Clear();
					httpClient.DefaultRequestHeaders.Accept
							.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					httpClient.DefaultRequestHeaders.Add("User-Agent", "Weather Report CLI");

					if (response == null || !response.IsSuccessStatusCode)
						throw new ExternalException("Oh no, something went wrong communicating with SMHI :-(");

					var jsonString = await response.Content.ReadAsStringAsync();
					return JsonConvert.DeserializeObject<LastHourTemperature>(jsonString);
				}

			throw new InvalidOperationException("Something went wrong retrieving object from SMHI.");
		}

		public async Task<LatestMonthsPrecipitationResponse> GetPrecipitationDataForLatestMonthsAsync()
		{

			using (var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(10) })
			using (HttpResponseMessage response =
				await httpClient.GetAsync(new Uri(Endpoints.LundPrecipitationLatestMonth)))
			{
				httpClient.DefaultRequestHeaders.Accept.Clear();
				httpClient.DefaultRequestHeaders.Accept
						.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				httpClient.DefaultRequestHeaders.Add("User-Agent", "Weather Report CLI");

				if (response == null || !response.IsSuccessStatusCode)
					throw new ExternalException("Oh no, something went wrong communicating with SMHI :-(");

				var jsonString = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<LatestMonthsPrecipitationResponse>(jsonString);
			}

			throw new InvalidOperationException("Something went wrong retrieving object from SMHI.");
		}
	}
}