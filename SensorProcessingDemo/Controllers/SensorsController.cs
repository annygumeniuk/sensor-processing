using Microsoft.AspNetCore.Mvc;
using SensorProcessingDemo.Services;
using SensorProcessingDemo.Models;
using SensorProcessingDemo.Common;
using SensorProcessingDemo.Attributes;
using System.Globalization;
using System.Security.Cryptography;

namespace SensorProcessingDemo.Controllers
{
    public class SensorsController : Controller
        {
        private readonly Data _dataService;

        public SensorsController(Data dataService)
        {
            _dataService = dataService;
        }

        // [temp] Funtion to generate new id (untill we have a database)
        public int RenderId()
        {
            return _dataService.sensors.Any() ? _dataService.sensors.Last().Id + 1 : 1;
        }

        // Function to generate sensors data        
        public void GenerateSensorData(string name, decimal min, decimal max)
        {
            var sensor = new Sensor(RenderId(), name, RenderData(min, max), RenderDate());
            _dataService.sensors.Add(sensor);

            // Limit the data list to the last 100 records
            if (_dataService.sensors.Count > 100)
            {
                _dataService.sensors.RemoveAt(0);
            }                        
        }

        // Function to generate random data for a sensor
        public decimal RenderData(decimal min, decimal max)
        {
            var generator = new Random();
            decimal value = new decimal(generator.NextDouble() * ((double)max - (double)min) + (double)min);
            return value;
        }

        // Function to render current date and convert it to needed format
        public DateTime RenderDate()
        {
            return DateTime.Now;
        }

        public IActionResult Index()
        {
            // Generate sensor data for different sensor types
            GenerateSensorData(Common.Constants.TEMPERATURE, Common.Constants.TEMP_MIN, Common.Constants.TEMP_MAX);
            GenerateSensorData(Common.Constants.HUMIDITY, Common.Constants.HUM_MIN, Common.Constants.HUM_MAX);
            GenerateSensorData(Common.Constants.LIGHTING, Common.Constants.LIGHT_MIN, Common.Constants.LIGHT_MAX);

            // Group sensors by Name and serialize data for the view
            var groupedData = _dataService.sensors
                .GroupBy(s => s.Name)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => new { timeGenerated = x.dateTime, x.Value }).ToList()
                );

            ViewBag.GroupedDataPoints = System.Text.Json.JsonSerializer.Serialize(groupedData);
            return View();
        }

        // API endpoint to get sensor data as JSON
        [Route("api/sensors/data")]
        [HttpGet]
        public IActionResult GetSensorData()
        {       
            var groupedData = _dataService.sensors
              .GroupBy(s => s.Name)
              .ToDictionary(
                  g => g.Key,
                  g => g.Select(x => new { dateTime = x.dateTime, x.Value }).ToList()
              );       
            return Json(groupedData);
        }
    }
}
