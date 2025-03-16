using Microsoft.AspNetCore.Mvc;
using SensorProcessingDemo.Models;
using SensorProcessingDemo.Common;

namespace SensorProcessingDemo.Controllers
{
    public class WindsMapController : Controller
    {
        private readonly IConfiguration _configuration;

        public WindsMapController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index(Location? model)
        {
            var apiKey = _configuration["WindyAPI:Key"];
            ViewData["WindyAPIKey"] = apiKey;

            if (ModelState.IsValid)
            {
                if (model.Latitude == null || model.Longitude == null)
                {
                    ViewData["Longitude"] = Common.Constants.LONG_KYIV;
                    ViewData["Latitude"] = Common.Constants.LAT_KYIV;
                }
                else
                {
                    ViewData["Longitude"] = model.Longitude;
                    ViewData["Latitude"] = model.Latitude;
                }                
            }            
            return View();
        }
    }
}
