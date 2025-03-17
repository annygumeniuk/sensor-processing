using SensorProcessingDemo.Models;
namespace SensorProcessingDemo.Services.Interfaces
{
    public interface ISensorDataService
    {
        Task GetSensorDataByDate(DateTime dateTime);
        Task<IEnumerable<Sensor>> GetAll();
        Task Create(Sensor sensor);
        Task Delete(int sensorId);
        Task DeleteAll();
    }
}
