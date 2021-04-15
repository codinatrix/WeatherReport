using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using WeatherReport.Cli.Integration.WeatherService;
using WeatherReport.Integration.WeatherService;

namespace WeatherReport.Cli
{
	internal class Program
	{
		public static async Task Main(string[] args)
		{
			Console.WriteLine("Welcome to the Weather Report CLI!");
			Console.WriteLine("The following is weather information collected from SMHI.");
			Console.WriteLine("Please note that this information is calculated from available data, but some weather stations may not have made a report in the alotted time.");
			Console.WriteLine();

			await PrintAverageTemperatureAsync();
			Console.WriteLine();

			await PrintTotalRainfallForLundAsync();
			Console.WriteLine();

            await PrintAllTemperaturesAsync();
			Console.WriteLine();

			Console.WriteLine();
			Console.WriteLine("Find the any key to exit");
			Console.ReadKey();
		}

		private async static Task<string> GetWeatherData(string endpoint)
        {
			using (var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(10) })
			using (HttpResponseMessage response =
				await httpClient.GetAsync(new Uri(endpoint)))
			{
				httpClient.DefaultRequestHeaders.Accept.Clear();
				httpClient.DefaultRequestHeaders.Accept
					.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				httpClient.DefaultRequestHeaders.Add("User-Agent", "Weather Report CLI");

				if (response == null || !response.IsSuccessStatusCode)
					throw new ExternalException("Oh no, something went wrong communicating with the Weather Service :-(");

				return await response.Content.ReadAsStringAsync();
			}
		}

		private async static Task PrintAverageTemperatureAsync()
		{
			try
			{
				string jsonString = await GetWeatherData(Endpoints.AverageTemperature);
				AverageTemperatureResponse temperature = JsonConvert.DeserializeObject<AverageTemperatureResponse>(jsonString);
				Console.WriteLine("The average temperature in all of Sweden the past hour is " + temperature.Average);
			}
			catch
			{
				Console.WriteLine("We would love to give you the average temperature for Sweden but unfortunately we couldn't.");
			}
		}

		private async static Task PrintTotalRainfallForLundAsync()
		{
			try
			{
				string jsonString = await GetWeatherData(Endpoints.LundPrecipitation);

				PrecipitationResponse precipitationResponse = JsonConvert.DeserializeObject<PrecipitationResponse>(jsonString);
				Console.WriteLine("Between " + precipitationResponse.From + " and " + precipitationResponse.To +
					", the total rainfall in Lund was " + precipitationResponse.RainfallInMm + " millimeters");
			}
			catch
			{
				Console.WriteLine("We would love to give you the total rainfall for Lund but unfortunately we couldn't.");
			}
		}



		private static Task PrintAllTemperaturesAsync()
        {
            try
			{
				Console.WriteLine("Temperature per weather station in Sweden. Press any key to cancel.");

				var cancellationTokenSource = new CancellationTokenSource();
				var cancellationToken = cancellationTokenSource.Token;

				var task = Task.Run(async () =>
				{
					// In case it was already cancelled
					cancellationToken.ThrowIfCancellationRequested();

					string jsonString = await GetWeatherData(Endpoints.AllTemperatures);
					AllTemperatureResponse temperatures = JsonConvert.DeserializeObject<AllTemperatureResponse>(jsonString);
					bool userCancelled = false;

					foreach (StationResponse station in temperatures.Stations)
					{
						Console.WriteLine(station.Name + ": " + station.Temperature);
						Thread.Sleep(100);
						if (cancellationToken.IsCancellationRequested)
                        {
							Console.WriteLine("You have cancelled the weather printout.");
							userCancelled = true;
							break;
						}
					}

					if (!userCancelled)
                    {
                        Console.WriteLine();
                        Console.WriteLine("That's all the weather data for today!");
                    }

				}, cancellationTokenSource.Token);
				
				Console.ReadKey();
				cancellationTokenSource.Cancel();
			}
           
			catch
			{
				Console.WriteLine("We would love to give you the temperature for all the weather stations in Sweden but unfortunately we couldn't.");
			}

			return Task.CompletedTask;
		}
    }
}
