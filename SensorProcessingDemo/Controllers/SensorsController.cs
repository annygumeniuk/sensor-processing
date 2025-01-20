using Microsoft.AspNetCore.Mvc;
using SensorProcessingDemo.Common;
using System.Collections.Concurrent;

namespace SensorProcessingDemo.Controllers
{
    public class SensorsController : Controller
    {
        private static readonly ConcurrentDictionary<string, List<(DateTime time, decimal value)>> SensorData =
                new ConcurrentDictionary<string, List<(DateTime, decimal)>>();

        public const int UpdateIntervalSeconds = 2; // Adjust as needed
        private static readonly Random Random = new();

        private static readonly Dictionary<string, (decimal min, decimal max)> SensorRanges =
            new Dictionary<string, (decimal min, decimal max)>
            {
                { "Temperature", (Constants.TEMP_MIN, Constants.TEMP_MAX) },
                { "Humidity", (Constants.HUM_MIN, Constants.HUM_MAX) },
                { "Lighting", (Constants.LIGHT_MIN, Constants.LIGHT_MAX) }
            };

        public SensorsController()
        {
            if (!isRunning)
            {
                isRunning = true;
                Task.Run(GenerateSensorDataLoop);
            }
        }
        
        [HttpGet("get-sensor-data")]
        public JsonResult GetSensorData()
        {
            return Json(SensorData.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Select(v => new { v.time, v.value }).ToList()));
        }

        private static bool isRunning = false;
       

        public IActionResult Index()
        {
            return View();
        }

        public async Task GenerateSensorDataLoop()
        {
            while (true)
            {
                foreach (var sensor in SensorRanges.Keys)
                {
                    var range = SensorRanges[sensor];
                    var value = Math.Round((decimal)(Random.NextDouble() * (double)(range.max - range.min)) + range.min, 2);

                    if (!SensorData.ContainsKey(sensor))
                        SensorData[sensor] = new List<(DateTime, decimal)>();

                    // Limit number of points for scrolling effect
                    if (SensorData[sensor].Count > 100)
                        SensorData[sensor].RemoveAt(0);

                    SensorData[sensor].Add((DateTime.Now, value));
                }

                await Task.Delay(UpdateIntervalSeconds * 1000);
            }
        }
    }
}
