using Microsoft.AspNetCore.Mvc;
using SensorProcessingDemo.Services;
using SensorProcessingDemo.Models;
using SensorProcessingDemo.Common;

namespace SensorProcessingDemo.Controllers
{
    public class SensorsController : Controller
    {
        private readonly Data _dataService;

        public SensorsController(Data dataService)
        {
            _dataService = dataService;
        }

        public IActionResult Index()
        {
            // Group sensors by Name and select their data points
            var groupedData = _dataService.sensors
                .GroupBy(s => s.Name)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => new { x.dateTime, x.Value }).ToList()
                );

            // Serialize grouped data to JSON format
            ViewBag.GroupedDataPoints = System.Text.Json.JsonSerializer.Serialize(groupedData);


            return View();
        }
    }
}
