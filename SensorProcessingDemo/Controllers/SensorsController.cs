using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SensorProcessingDemo.Common;
using SensorProcessingDemo.Repositories.Interfaces;
using System.Collections.Concurrent;
using SensorProcessingDemo.Models;
using SensorProcessingDemo.Services.Interfaces;

namespace SensorProcessingDemo.Controllers
{
    [Authorize]
    public class SensorsController : Controller
    {
        private static readonly ConcurrentDictionary<string, List<(DateTime time, decimal value)>> SensorData =
                new ConcurrentDictionary<string, List<(DateTime, decimal)>>();
        
        private static readonly Random Random = new();
        private static bool isRunning = false;
        
        private readonly IEntityRepository<Monitoring> _monitoringContext;
        private readonly IEntityRepository<Sensor> _sensorContext;       
        private readonly ICurrentUserService _currentUserService;
        private readonly IMonitoringService _monitoringService;

        private static readonly Dictionary<string, (decimal min, decimal max)> SensorRanges =
            new Dictionary<string, (decimal min, decimal max)>
            {
                { "Temperature", (Constants.TEMP_MIN, Constants.TEMP_MAX) },
                { "Humidity", (Constants.HUM_MIN, Constants.HUM_MAX) },
                { "Lighting", (Constants.LIGHT_MIN, Constants.LIGHT_MAX) }
            };

        public SensorsController(ICurrentUserService currentUserService, IMonitoringService service)
        {
            _currentUserService = currentUserService;
            _monitoringService = service;

            if (isRunning)
            {
                int userId = Convert.ToInt32(_currentUserService.GetUserId());
                var isExist = _monitoringService.ExistWithUserId(userId);
                if (!isExist)
                {
                    _monitoringService.StartMonitoring(userId);
                }

                Task.Run(GenerateSensorDataLoop);
            }
            else
            {
                
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("get-sensor-data")]
        public JsonResult GetSensorData()
        {
            return Json(SensorData.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Select(v => new { v.time, v.value }).ToList()));
        }

        public bool CheckIfValueInRange(decimal value, decimal max, decimal min)
        { 
            return value <= max && value >= min;
        }

        public void AddAlertRecord(decimal value, decimal max, decimal min)
        {
            bool isInRange = CheckIfValueInRange(value, max, min);

            if (!isInRange)
            {
                // TODO: Add record to alert collector
            }
        }

        public async Task GenerateSensorDataLoop()  
        {                       
            int counter = 0;
            while (true)
            {
                foreach (var sensor in SensorRanges.Keys)
                {
                    counter++;
                    
                    var range = SensorRanges[sensor];
                    var value = Math.Round((decimal)(Random.NextDouble() * (double)(range.max - range.min)) + range.min, 2);


                    if (!SensorData.ContainsKey(sensor))
                        SensorData[sensor] = new List<(DateTime, decimal)>();

                    // Limit number of points for scrolling effect
                    if (SensorData[sensor].Count > 100)
                        SensorData[sensor].RemoveAt(0);

                    // To generate a value which is out of the range
                    if (counter == 10)
                        value = range.max + 10;

                    AddAlertRecord(value, range.max, range.min);

                    SensorData[sensor].Add((DateTime.Now, value));
                }

                await Task.Delay(Common.Constants.UpdateIntervalSeconds * 1000);
            }
        }
    }
}