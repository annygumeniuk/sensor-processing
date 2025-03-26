using Microsoft.AspNetCore.Mvc;
using SensorProcessingDemo.Services.Implementations;

namespace SensorProcessingDemo.Controllers
{
    public class WeatherReceiverController : Controller
    {
        private readonly WeatherService _weatherService;

        public WeatherReceiverController(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetWeather(double lat, double lon)
        {
            try
            {
                var weather = await _weatherService.GetWeatherAsync(lat, lon);
                return Json(weather);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
    }
}
