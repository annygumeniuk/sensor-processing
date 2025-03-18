using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SensorProcessingDemo.Common;
using SensorProcessingDemo.Repositories.Interfaces;
using System.Collections.Concurrent;
using SensorProcessingDemo.Models;
using SensorProcessingDemo.Services.Interfaces;
using SensorProcessingDemo.ModelFilters;

namespace SensorProcessingDemo.Controllers
{
    [Authorize]
    public class SensorsController : Controller
    {
        private static readonly ConcurrentDictionary<string, List<(DateTime time, decimal value)>> SensorData =
                new ConcurrentDictionary<string, List<(DateTime, decimal)>>();
        
        private static readonly Random Random = new();
        private static bool isRunning = false;
                
        private readonly ISensorDataService _sensorDataService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMonitoringService _monitoringService;

        private static readonly Dictionary<string, (decimal min, decimal max)> SensorRanges =
            new Dictionary<string, (decimal min, decimal max)>
            {
                { "Temperature", (Constants.TEMP_MIN, Constants.TEMP_MAX) },
                { "Humidity", (Constants.HUM_MIN, Constants.HUM_MAX) },
                { "Lighting", (Constants.LIGHT_MIN, Constants.LIGHT_MAX) }
            };

        public SensorsController(
            ISensorDataService sensorDataService,
            ICurrentUserService currentUserService,
            IMonitoringService monitoringService)
        {            
            _sensorDataService = sensorDataService;
            _currentUserService = currentUserService;
            _monitoringService = monitoringService;
        }

        [HttpPost("toggle-monitoring")]
        public async Task<JsonResult> ToggleMonitoring()
        {
            int userId = Convert.ToInt32(_currentUserService.GetUserId());

            isRunning = !isRunning;

            if (isRunning)
            {
                SensorData.Clear(); // clear previous data
                await _monitoringService.StartMonitoring(userId);
                _ = Task.Run(async () => await GenerateSensorDataLoop(userId));
            }
            else
            {
                await _monitoringService.StopMonitoring(userId);
            }

            return Json(new { isRunning });
        }


        public async Task Run()
        {
            int userId = Convert.ToInt32(_currentUserService.GetUserId());

            var isExist = await _monitoringService.CurrentExistWithUserId(userId);

            if (isRunning)
            {
                if (isExist == null)
                {
                    await _monitoringService.StartMonitoring(userId);
                    _ = Task.Run(async () => await GenerateSensorDataLoop(userId));
                }
            }
            else
            {
                if (isExist != null)
                {
                    await _monitoringService.StopMonitoring(userId);
                }
            }
        }

        public async Task GenerateSensorDataLoop(int currentUserId)
        {
            while (isRunning)
            {
                foreach (var sensor in SensorRanges.Keys)
                {
                    var range = SensorRanges[sensor];
                    var value = Math.Round((decimal)(Random.NextDouble() * (double)(range.max - range.min)) + range.min, 2);

                    if (!SensorData.ContainsKey(sensor))
                        SensorData[sensor] = new List<(DateTime, decimal)>();

                    if (SensorData[sensor].Count > 100)
                        SensorData[sensor].RemoveAt(0);

                    try 
                    {
                        SensorData[sensor].Add((DateTime.Now, value));
                        Sensor sens = new Sensor(currentUserId, sensor, value);

                        await _sensorDataService.Create(sens);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }

                await Task.Delay(Common.Constants.UpdateIntervalSeconds * 1000);
            }
        }

        public async Task<IActionResult> Index()
        {
            await Run();
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

        public async Task<IActionResult> GetAll(SensorFilter filter)
        {
            int userId = Convert.ToInt32(_currentUserService.GetUserId());
            var sensors = await _sensorDataService.GetAll(userId, filter);

            ViewBag.Sensors = sensors;

            return View(filter);
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(int sensorId)
        {            
            await _sensorDataService.Delete(sensorId);
            return RedirectToAction("GetAll");
        }
    }
}