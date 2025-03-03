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

        public async Task StartMonitoring(int userId)
        {
            _logger.LogInformation("Trying to start the monitoring.");

            Monitoring monitoring = new Monitoring()
            {
                UserId = userId,
                MonitoringStartedAt = DateTime.Now,
                MonitoringStoppedAt = null,
            };

            _logger.LogInformation($"Monitoring started at {monitoring.MonitoringStartedAt} by user {userId}.");
            await _monitoringContext.AddAsync(monitoring);
            _logger.LogInformation("Monitoring data was added to db.");
        }

        public async Task StopMonitoring(int userId)
        {
            var record = await _monitoringContext.GetFirstOrDefault(x => x.UserId == userId && x.MonitoringStoppedAt == null);

            if (record != null)
            {
                record.MonitoringStoppedAt = DateTime.Now;
                await _monitoringContext.UpdateAsync(record);
            }
        }

        public async Task<IEnumerable<Monitoring>> GetByUserId(int userId)
        {
            return await _monitoringContext.FindAsync(x => x.UserId == userId);
        }

        public async Task<Monitoring?> CurrentExistWithUserId(int userId)
        {
            try
            {
                return await _monitoringContext.GetFirstOrDefault(x => x.UserId == userId && x.MonitoringStoppedAt == null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in CurrentExistWithUserId: {ex.Message}");
                return null;
            }
        }

        Task IMonitoringService.GetByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public bool ExistWithUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Monitoring>> GetAllActiveMonitorings()
        {
            return await _monitoringContext.FindAsync(x => x.MonitoringStoppedAt == null);
        }

    }
}
