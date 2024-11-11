using Microsoft.AspNetCore.Mvc;
using SensorProcessingDemo.Services;
using SensorProcessingDemo.Models;

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
            // Create a view model and assign sensor data to it
            var viewModel = new SensorViewModel
            {
                Sensors = _dataService.sensors
            };

            // Optionally, set other properties, e.g., SensorType
            viewModel.SensorType = "Temperature"; // Or another value based on the context

            return View(viewModel);
        }
    }
}
