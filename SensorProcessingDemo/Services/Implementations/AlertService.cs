using SensorProcessingDemo.Common;
using SensorProcessingDemo.Models;
using SensorProcessingDemo.Repositories.Interfaces;
using SensorProcessingDemo.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SensorProcessingDemo.Services.Implementations
{
    public class AlertService : IAlertService
    {
        private readonly ILogger<AlertService> _logger;
        private readonly IEntityRepository<AlertCollector> _alertContext;

        public AlertService(ILogger<AlertService> logger, IEntityRepository<AlertCollector> alertContext)
        { 
            _logger = logger;
            _alertContext = alertContext;
        }
        
        public async Task<IEnumerable<AlertCollector>> GetAll(int userId)
        {
            _logger.LogInformation("Trying to get sensors data for current user from db.");
            var predicate = PredicateBuilder.True<AlertCollector>().And(s => s.Sensor.UserId == userId);

            return await _alertContext.FindAsync(predicate, query => query.Include(a => a.Sensor)); 
        }

        public Task Delete(int alertId)
        {
            _logger.LogInformation("Trying to delete alert data from db.");

            try
            {
                _alertContext.DeleteAsync(alertId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error message: {ex}");
            }

            _logger.LogInformation($"Alert was deleted.");

            return Task.CompletedTask;
        }

        public Task Create(AlertCollector alert)
        {
            _logger.LogInformation("Trying to add new alert in db.");
            try
            {
                _alertContext.AddAsync(alert);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error message: {ex}");
            }

            _logger.LogInformation($"New alert was added to db.");

            return Task.CompletedTask;
        }
    }
}
