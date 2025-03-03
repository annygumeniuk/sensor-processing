using SensorProcessingDemo.Controllers;
using SensorProcessingDemo.Models;
using SensorProcessingDemo.Repositories.Interfaces;
using SensorProcessingDemo.Services.Interfaces;

namespace SensorProcessingDemo.Services.Implementations
{
    public class MonitoringService : IMonitoringService
    {
        private readonly ILogger<MonitoringService> _logger;
        private readonly IEntityRepository<Monitoring> _monitoringContext;

        public MonitoringService(IEntityRepository<Monitoring> entityRepository, ILogger<MonitoringService> logger)
        {
            _monitoringContext = entityRepository;
            _logger = logger;
        }

        public void StartMonitoring(int userId)
        {
            _logger.LogInformation("Trying to start the monitoring.");
            
            Monitoring monitoring = new Monitoring()
            {
                UserId = userId,
                MonitoringStartedAt = DateTime.Now,
                MonitoringStoppedAt = null,
            };

            _monitoringContext.AddAsync(monitoring);
            _monitoringContext.SaveAsync();

            _logger.LogInformation("Monitoring was started.");            
        }

        public async Task StopMonitoring(int userId)
        {            
            var record = await _monitoringContext.GetFirstOrDefault(x => x.UserId == userId && x.MonitoringStoppedAt == null);

            if (record != null)
            {                
                record.MonitoringStoppedAt = DateTime.Now;             
                await _monitoringContext.SaveAsync();
            }
        }

        public bool ExistWithUserId(int userId)
        {
            var record = GetByUserId(userId);
            return record != null;
        }

        public async Task<Monitoring?> CurrentExistWithUserId(int userId)
        {
            var record = await _monitoringContext.GetFirstOrDefault(x => x.UserId == userId && x.MonitoringStartedAt == x.MonitoringStoppedAt);

            return record != null ? record : null;
        }

        public Task GetByUserId(int userId)
        {
            return _monitoringContext.FindAsync(x => x.UserId == userId);
        }
    }
}
