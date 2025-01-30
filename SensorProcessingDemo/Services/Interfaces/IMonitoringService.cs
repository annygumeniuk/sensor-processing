using SensorProcessingDemo.Models;

namespace SensorProcessingDemo.Services.Interfaces
{
    public interface IMonitoringService
    {
        Task GetByUserId(int userId);
        void StartMonitoring(int userId);
        Task StopMonitoring(int userId);
        bool ExistWithUserId(int userId);
    }
}