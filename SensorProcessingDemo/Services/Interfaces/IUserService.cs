using SensorProcessingDemo.Models;

namespace SensorProcessingDemo.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAll(int userId);
        Task<User> GetUser(int userId);
    }
}
