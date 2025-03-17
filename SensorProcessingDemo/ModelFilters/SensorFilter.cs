using SensorProcessingDemo.Models;
namespace SensorProcessingDemo.ModelFilters
{
    public class SensorFilter
    {
        public bool DisplayTemp  { get; set; } = false;
        public bool DisplayHum   { get; set; } = false;
        public bool DisplayLight { get; set; } = false;

        public List<Sensor> Sensors { get; set; } = new();
    }
}
