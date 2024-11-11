using SensorProcessingDemo.Attributes;

namespace SensorProcessingDemo.Models
{
    public class Sensor
    {
        public int     Id    { get; set; }
        public string  Name  { get; set; }
        public decimal Value { get; set; }

        [DateTimeFormat("yyyy-MM-dd-HH-mm-ss")]
        public DateTime dateTime { get; set; }
    }
}
