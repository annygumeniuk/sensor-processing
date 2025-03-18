using SensorProcessingDemo.Models;

namespace SensorProcessingDemo.Services.Interfaces
{
    public interface IAlertService
    {
        Task<IEnumerable<AlertCollector>> GetAll(int userId);
        Task Delete(int alertId);
        Task Create(AlertCollector alert);
    }
}
