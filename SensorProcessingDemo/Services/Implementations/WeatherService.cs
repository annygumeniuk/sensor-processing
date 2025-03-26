using SensorProcessingDemo.Models;
using System.Text.Json;

namespace SensorProcessingDemo.Services.Implementations
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        
        private const string BASE_URL = "https://api.openweathermap.org/data/2.5/weather";

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<WeatherResponse> GetWeatherAsync(double latitude, double longitude)
        {
            var apiKey = _configuration["OpenWeather:Key"];
            
            string url = $"{BASE_URL}?lat={latitude}&lon={longitude}&appid={apiKey}&units=metric";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to fetch weather data: {response.StatusCode}");
            }

            string json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<WeatherResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
