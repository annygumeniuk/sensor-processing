using SensorProcessingDemo.Models;

namespace SensorProcessingDemo.Auth
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}
