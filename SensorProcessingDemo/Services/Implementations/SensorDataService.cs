using SensorProcessingDemo.Models;
using SensorProcessingDemo.ModelFilters;
using SensorProcessingDemo.Repositories.Interfaces;
using SensorProcessingDemo.Services.Interfaces;
using SensorProcessingDemo.Common;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;

namespace SensorProcessingDemo.Services.Implementations
{
    public class SensorDataService : ISensorDataService
    {
        private readonly ILogger<SensorDataService> _logger;        
        private readonly IEntityRepository<Sensor> _sensorContext;

        public SensorDataService(ILogger<SensorDataService> logger,                              
                                 IEntityRepository<Sensor> sensorContext)
        {
            _logger = logger;           
            _sensorContext = sensorContext;
        }   

        public Task Create(Sensor sensor)
        {
            _logger.LogInformation("Trying to add new sensor data in db.");

            try
            {
                _sensorContext.AddAsync(sensor);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error message: {ex}");
            }

            _logger.LogInformation($"New sensor data {sensor.Name} - {sensor.Value} was added.");

            return Task.CompletedTask;
        }

        public Task Delete(int sensorId)
        {
            _logger.LogInformation("Trying to delete sensor data from db.");

            try
            {
                _sensorContext.DeleteAsync(sensorId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error message: {ex}");
            }

            _logger.LogInformation($"Sensor with id {sensorId} was deleted.");

            return Task.CompletedTask;
        }

        public Task DeleteAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Sensor>> GetAll(int userId, SensorFilter filter)
        {
            _logger.LogInformation("Trying to get sensors data for current user from db.");

            var predicate = PredicateBuilder.True<Sensor>().And(s => s.UserId == userId);
            var selectedTypes = new List<string>();

            if (filter.DisplayTemp) selectedTypes.Add(Enums.SENSORNAME.Temperature.ToString());
            if (filter.DisplayHum) selectedTypes.Add(Enums.SENSORNAME.Humidity.ToString());
            if (filter.DisplayVis) selectedTypes.Add(Enums.SENSORNAME.Visibility.ToString());
            if (filter.DisplayAtmPress) selectedTypes.Add(Enums.SENSORNAME.AtmosphericPressure.ToString());
            {
                
            }

            if (selectedTypes.Any())
            {
                _logger.LogInformation($"Filtering by sensor types: {string.Join(", ", selectedTypes)}");
                predicate = predicate.And(s => selectedTypes.Contains(s.Name));
            }
            else
            {
                _logger.LogInformation("No filters applied, returning all sensor data.");
            }

            if (filter.DateFrom != default)
            {
                _logger.LogInformation($"Filtering from date: {filter.DateFrom}");
                predicate = predicate.And(s => s.dateTime >= filter.DateFrom);
            }

            if (filter.DateTo != default)
            {
                _logger.LogInformation($"Filtering to date: {filter.DateTo}");
                predicate = predicate.And(s => s.dateTime <= filter.DateTo);
            }

            return await _sensorContext.FindAsync(predicate);
        }

        public Task<Sensor> GetSensorById(int sensorId)
        {
            _logger.LogInformation("Trying to get sensor data by id from db.");
            var sensor = _sensorContext.GetByIdAsync(sensorId);

            return sensor;
        }

        public Task GetSensorDataByDate(DateTime dateTime)
        {
            throw new NotImplementedException();
        }
        
        public async Task<byte[]> ExportSensorDataAsync()
        {
            _logger.LogInformation("Trying to get sensor data for export.");
            var sensors = await _sensorContext.SelectAsync(s => new { s.Name, s.Value, s.dateTime });
            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

            await csvWriter.WriteRecordsAsync(sensors);
            await streamWriter.FlushAsync();
            
            _logger.LogInformation("Passing data for export.");
            
            return memoryStream.ToArray();
        }
    }
}
