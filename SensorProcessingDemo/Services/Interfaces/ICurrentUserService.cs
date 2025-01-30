namespace SensorProcessingDemo.Services.Interfaces
{
    public interface ICurrentUserService
    {
        string GetUserId();
        string GetUserEmail();
        bool IsAuthenticated();
    }
}
