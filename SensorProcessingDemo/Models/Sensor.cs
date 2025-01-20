using SensorProcessingDemo.Attributes;
using SensorProcessingDemo.Common;

namespace SensorProcessingDemo.Models
{
    public class Sensor
    {
        public int     Id    { get; set; }
        public string  Name  { get; set; }
        public decimal Value { get; set; }        
        public DateTime dateTime { get; set; }


        public Sensor(int id, string name, decimal value, DateTime time)
        { 
            Id = id;
            Name = name;
            Value = value;
            dateTime = time;
        }

        public Sensor() { }

       //  Method to check if generated value is in range
        public bool CheckBounds(decimal min, decimal max, decimal value)
        {
            return (value >= min && value <= max);            
        }       
    }
}
