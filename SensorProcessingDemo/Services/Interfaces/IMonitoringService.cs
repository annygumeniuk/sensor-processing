using SensorProcessingDemo.Models;

namespace SensorProcessingDemo.Services.Interfaces
{
    public interface IMonitoringService
    {
        Task GetByUserId(int userId);
        Task StartMonitoring(int userId);
        Task StopMonitoring(int userId);
        bool ExistWithUserId(int userId);
        Task<Monitoring?> CurrentExistWithUserId(int userId);
    }
}