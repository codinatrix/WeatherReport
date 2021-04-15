using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using WeatherReport.Web.Integration.WeatherService;

namespace WeatherReport.Cli
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			ConnectAsync().Wait();

			Console.WriteLine();
			Console.WriteLine("Find the any key to exit");
			Console.ReadKey();
		}

		private static async Task ConnectAsync()
		{

            try
			{
                using (var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(10) })
                using (HttpResponseMessage response =
                    await httpClient.GetAsync(new Uri("https://localhost:44305/api/temperatures/average")))
                {
					httpClient.DefaultRequestHeaders.Accept.Clear();
					httpClient.DefaultRequestHeaders.Accept
						.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					httpClient.DefaultRequestHeaders.Add("User-Agent", "Weather Report CLI");

					if (response == null || !response.IsSuccessStatusCode)
						throw new ExternalException("Oh no, something went wrong communicating with the Weather Service :-(");

					var jsonString = await response.Content.ReadAsStringAsync();
					TemperatureResponse temperature = JsonConvert.DeserializeObject<TemperatureResponse>(jsonString);
					Console.WriteLine("The average temperature in all of Sweden the past hour is " + temperature.AverageTemperature);
					Console.WriteLine("Please note that the average is calculated from available data, but some weather stations may not have made a report in the last hour.");
				}
            }
			catch (HttpRequestException e)
			{
				Console.WriteLine(e);
			}
		}
	}
}
