using SensorProcessingDemo.ModelFilters;
using SensorProcessingDemo.Models;
namespace SensorProcessingDemo.Services.Interfaces
{
    public interface ISensorDataService
    {
        Task GetSensorDataByDate(DateTime dateTime);
        Task<IEnumerable<Sensor>> GetAll(int userId, SensorFilter filter);
        Task Create(Sensor sensor);
        Task Delete(int sensorId);
        Task DeleteAll();
        Task<Sensor> GetSensorById(int sensorId);
    }
}
