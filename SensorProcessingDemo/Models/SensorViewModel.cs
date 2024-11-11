namespace SensorProcessingDemo.Models
{
    public class SensorViewModel
    {
        public List<Sensor> Sensors { get; set; }
        public string SensorType { get; set; }
        public int TotalSensors => Sensors?.Count ?? 0;
    }
}
