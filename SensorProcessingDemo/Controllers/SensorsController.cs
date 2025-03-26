using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SensorProcessingDemo.Common;
using SensorProcessingDemo.Repositories.Interfaces;
using System.Collections.Concurrent;
using SensorProcessingDemo.Models;
using SensorProcessingDemo.Services.Interfaces;
using SensorProcessingDemo.ModelFilters;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static SensorProcessingDemo.Common.Enums;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using SensorProcessingDemo.Services.Implementations;

namespace SensorProcessingDemo.Controllers
{
    [Authorize]
    public class SensorsController : Controller
    {
        private static readonly ConcurrentDictionary<string, List<(DateTime time, decimal value)>> SensorData =
                new ConcurrentDictionary<string, List<(DateTime, decimal)>>();
        
        private static readonly Random Random = new();
        private static bool isRunning = false;
                
        private readonly ISensorDataService  _sensorDataService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMonitoringService  _monitoringService;
        private readonly IAlertService       _alertService;
        private readonly WeatherService      _weatherService;

        private static readonly Dictionary<string, (decimal min, decimal max)> SensorRanges =
            new Dictionary<string, (decimal min, decimal max)>
            {
                { SENSORNAME.Temperature.ToString(),         (Constants.TEMP_MIN,      Constants.TEMP_MAX) },
                { SENSORNAME.Humidity.ToString(),            (Constants.HUM_MIN,       Constants.HUM_MAX) },
                { SENSORNAME.Visibility.ToString(),          (Constants.VIS_MIN,       Constants.VIS_MAX) },
                { SENSORNAME.AtmosphericPressure.ToString(), (Constants.ATM_PRESS_MIN, Constants.ATM_PRESS_MAX) }
            };

        public SensorsController(
            ISensorDataService sensorDataService,
            ICurrentUserService currentUserService,
            IMonitoringService monitoringService,
            IAlertService alertService,
            WeatherService weatherService)
        {
            _sensorDataService = sensorDataService;
            _currentUserService = currentUserService;
            _monitoringService = monitoringService;
            _alertService = alertService;
            _weatherService = weatherService;
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
                // Fetch real weather data                
                var weatherData = await _weatherService.GetWeatherAsync(Constants.LAT_KYIV, Constants.LONG_KYIV);

                if (weatherData != null)
                {
                    // Map real values to sensor names
                    var sensorValues = new Dictionary<string, decimal>
                    {
                        { "Temperature", (decimal)weatherData.Main.Temp },
                        { "Humidity", weatherData.Main.Humidity },
                        { "Visibility", weatherData.Visibility },
                        { "AtmosphericPressure", weatherData.Main.Pressure }
                    };

                    foreach (var sensor in sensorValues.Keys)
                    {
                        decimal value = sensorValues[sensor];

                        if (!SensorData.ContainsKey(sensor))
                            SensorData[sensor] = new List<(DateTime, decimal)>();

                        if (SensorData[sensor].Count > 100)
                            SensorData[sensor].RemoveAt(0);

                        SensorData[sensor].Add((DateTime.Now, value));

                        // Store sensor data in the database
                        Sensor sens = new Sensor(currentUserId, sensor, value);
                        await _sensorDataService.Create(sens);

                        // Check for alerts
                        if (value < SensorRanges[sensor].min || value > SensorRanges[sensor].max)
                        {
                            var alert = new AlertCollector { SensorId = sens.Id, Sensor = sens };
                            await _alertService.Create(alert);
                        }
                    }
                }
                await Task.Delay(Constants.UpdateIntervalSeconds * 1000);
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

        [HttpGet]
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

        [HttpGet("export")]
        public async Task<IActionResult> ExportSensors()
        {
            var fileBytes = await _sensorDataService.ExportSensorDataAsync();
            return File(fileBytes, "text/csv", "sensors_data.csv");
        }
    }
}