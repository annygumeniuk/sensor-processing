using SensorProcessingDemo.Models;
using SensorProcessingDemo.Repositories.Interfaces;
using SensorProcessingDemo.Services.Interfaces;

namespace SensorProcessingDemo.Services.Implementations
{
    public class SensorDataService : ISensorDataService
    {
        private readonly ILogger<SensorDataService> _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IEntityRepository<Sensor> _sensorContext;

        public SensorDataService(ILogger<SensorDataService> logger, 
                                 ICurrentUserService currentUser, 
                                 IEntityRepository<Sensor> sensorContext)
        {
            _logger = logger;
            _currentUserService = currentUser;
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

        public Task<IEnumerable<Sensor>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task GetSensorDataByDate(DateTime dateTime)
        {
            throw new NotImplementedException();
        }
    }
}
