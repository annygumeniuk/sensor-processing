using SensorProcessingDemo.Models;
using SensorProcessingDemo.Repositories.Interfaces;
using SensorProcessingDemo.Services.Interfaces;

namespace SensorProcessingDemo.Services.Implementations
{
    public class MonitoringService : IMonitoringService
    {
        private readonly IEntityRepository<Monitoring> _monitoringContext;

        public MonitoringService(IEntityRepository<Monitoring> entityRepository)
        {
            _monitoringContext = entityRepository;
        }

        public void StartMonitoring(int userId)
        {
            Monitoring monitoring = new Monitoring()
            {
                UserId = userId,
                MonitoringStartedAt = DateTime.Now,
                MonitoringStoppedAt = DateTime.Now,
            };

            _monitoringContext.AddAsync(monitoring);
            _monitoringContext.SaveAsync();
        }

        public async Task StopMonitoring(int userId)
        {            
            var record = await _monitoringContext.GetFirstOrDefault(x => x.UserId == userId && x.MonitoringStartedAt == x.MonitoringStoppedAt);

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

        public Task GetByUserId(int userId)
        {
            return _monitoringContext.FindAsync(x => x.UserId == userId);
        }
    }
}
