using Microsoft.AspNetCore.Mvc;

namespace SensorProcessingDemo.Controllers
{
    public class WindsMapController : Controller
    {
        private readonly IConfiguration _configuration;

        public WindsMapController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var apiKey = _configuration["WindyAPI:Key"]; // Read from appsettings.json or secrets
            ViewData["WindyAPIKey"] = apiKey;
            return View();
        }
    }
}
