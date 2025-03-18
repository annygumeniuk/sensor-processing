using SensorProcessingDemo.Attributes;
using SensorProcessingDemo.Models;
namespace SensorProcessingDemo.ModelFilters
{
    public class SensorFilter
    {
        public bool DisplayTemp  { get; set; } = false;
        public bool DisplayHum   { get; set; } = false;
        public bool DisplayLight { get; set; } = false;

        [DateTimeFormat("yyyy-MM-dd HH:mm:ss")]
        public DateTime DateFrom { get; set; }
        [DateTimeFormat("yyyy-MM-dd HH:mm:ss")]
        public DateTime DateTo   { get; set; }

        public List<Sensor> Sensors { get; set; } = new();
    }
}
