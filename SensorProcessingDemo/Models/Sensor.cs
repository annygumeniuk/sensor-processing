using SensorProcessingDemo.Attributes;
using SensorProcessingDemo.Common;

namespace SensorProcessingDemo.Models
{
    public class Sensor
    {
        public int     Id    { get; set; }
        public string  Name  { get; set; }
        public decimal Value { get; set; }

        [DateTimeFormat("yyyy-MM-dd-HH-mm-ss")]
        public DateTime dateTime { get; set; }


        public Sensor(int id, string name, decimal value, DateTime time)
        { 
            Id = id;
            Name = name;
            Value = value;
            dateTime = time;
        }

        public Sensor() { }

        public bool CheckBounds(decimal min, decimal max, decimal value)
        {
            return (value >= min && value <= max);            
        }

        public bool IsValid()
        {
            var sensorRanges = new Dictionary<string, (decimal min, decimal max)>
            {
                { Common.Constants.TEMPERATURE, (Common.Constants.TEMP_MIN,  Common.Constants.TEMP_MAX) },
                { Common.Constants.HUMIDITY,    (Common.Constants.HUM_MIN,   Common.Constants.HUM_MAX) },
                { Common.Constants.LIGHTING,    (Common.Constants.LIGHT_MIN, Common.Constants.LIGHT_MAX) }
            };


            if (sensorRanges.ContainsKey(this.Name))
            {
                var range = sensorRanges[this.Name];
                return CheckBounds(range.min, range.max, this.Value);
            }

            return false;
        }
    }
}
