namespace SensorProcessingDemo.Auth
{
    public class JwtOptions
    {
        public string SecretKey { get; set; } = string.Empty;
        public int ExpiresHours { set; get; }
    }
}
