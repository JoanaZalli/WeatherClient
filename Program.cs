using Microsoft.AspNetCore.SignalR.Client;

namespace WeatherClient
{
	class Program
	{
		private static HubConnection _connection;

		static async Task Main(string[] args)
		{
			var hubUrl = "https://localhost:5001/weatherHub";

			_connection = new HubConnectionBuilder()
				.WithUrl(hubUrl)
				.Build();

			_connection.On<WeatherNotification>("ReceiveNotification", (notification) =>
			{
				Console.WriteLine($"Weather Update for {notification.City} on {notification.Date}:");
				Console.WriteLine($"Description: {notification.Description}");
				Console.WriteLine($"Temperature: {notification.Temperature}°C");
				Console.WriteLine($"TempMax: {notification.TempMax}");
				Console.WriteLine($"TempMin: {notification.TempMin}");
				Console.WriteLine($"Humidity: {notification.Humidity}%");
				Console.WriteLine($"Wind Speed: {notification.WindSpeed} km/h");
			});

			try
			{
				await _connection.StartAsync();
				Console.WriteLine("Connection started. Listening for notifications...");

				// Keep the console application running
				Console.WriteLine("Press any key to exit...");
				Console.ReadKey();

				await _connection.StopAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error connecting to SignalR hub: {ex.Message}");
			}
		}
	}
}
