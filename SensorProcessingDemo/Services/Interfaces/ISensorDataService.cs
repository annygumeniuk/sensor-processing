using SensorProcessingDemo.ModelFilters;
using SensorProcessingDemo.Models;
namespace SensorProcessingDemo.Services.Interfaces
{
    public interface ISensorDataService
    {
        Task GetSensorDataByDate(DateTime dateTime);
        Task<IEnumerable<Sensor>> GetAll(int userId, SensorFilter filter);
        Task Create(Sensor sensor, Dictionary<string, (decimal min, decimal max)> SensorRanges);
        Task Delete(int sensorId);
        Task DeleteAll();
        Task<Sensor> GetSensorById(int sensorId);
        Task<byte[]> ExportSensorDataAsync();
    }
}
