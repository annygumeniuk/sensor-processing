using Microsoft.AspNetCore.Mvc;
using SensorProcessingDemo.Services;
using SensorProcessingDemo.Models;
using SensorProcessingDemo.Common;
using SensorProcessingDemo.Attributes;
using System.Globalization;

namespace SensorProcessingDemo.Controllers
{
    public class SensorsController : Controller
    {
        private readonly Data _dataService;        

        public SensorsController(Data dataService)
        {
            _dataService = dataService;
        }

        // temp funtion to generate new id (untill we have a database)
        public int RenderId()
        {                        
            return _dataService.sensors.Last().Id + 1;
        }

        // Function to imitate sensors update
        public decimal RenderData(decimal min, decimal max)
        {
            var generator = new Random();
            decimal value = new decimal(generator.NextDouble() * ((double)max - (double)min) + (double)min);

            return value;
        }
        
        public DateTime RenderDate() 
        {
            string DateTimeFormatted = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            DateTime time = DateTime.ParseExact(DateTimeFormatted, "yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture);
            return time;
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

            // Render data
            decimal temp  = RenderData(Common.Constants.TEMP_MIN,  Common.Constants.TEMP_MAX);
            decimal hum   = RenderData(Common.Constants.HUM_MIN,   Common.Constants.HUM_MAX);
            decimal light = RenderData(Common.Constants.LIGHT_MIN, Common.Constants.LIGHT_MAX);                

            // Add data to the list (database in future)
            _dataService.sensors.Add(new Sensor(RenderId(), Common.Constants.TEMPERATURE, temp, RenderDate()));
            _dataService.sensors.Add(new Sensor(RenderId(), Common.Constants.HUMIDITY, hum, RenderDate()));
            _dataService.sensors.Add(new Sensor(RenderId(), Common.Constants.LIGHTING, light, RenderDate()));

            // Serialize grouped data to JSON format                              
            ViewBag.GroupedDataPoints = System.Text.Json.JsonSerializer.Serialize(groupedData);
            
            return View();                        
        }
    }
}
