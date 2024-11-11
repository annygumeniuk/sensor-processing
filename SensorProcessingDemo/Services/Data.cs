using SensorProcessingDemo.Models;
using System.Globalization;
namespace SensorProcessingDemo.Services
{
    public class Data
    {
        public List<Sensor> sensors = new List<Sensor>()
        { 
            new Sensor() { Id = 1, Name = "Temperature", Value = 20m,   dateTime = DateTime.ParseExact("2024-11-12-08-30-45", "yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture)},
            new Sensor() { Id = 2, Name = "Temperature", Value = 18.5m, dateTime = DateTime.ParseExact("2024-11-12-10-30-20", "yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture)},                        
            
            new Sensor() { Id = 3, Name = "Humidity", Value = 35m, dateTime = DateTime.ParseExact("2024-11-12-08-30-45", "yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture)},
            new Sensor() { Id = 4, Name = "Humidity", Value = 55m, dateTime = DateTime.ParseExact("2024-11-12-10-30-20", "yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture)},

            new Sensor() { Id = 5, Name = "Lighting", Value = 500m, dateTime = DateTime.ParseExact("2024-11-12-08-30-45", "yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture)},
            new Sensor() { Id = 6, Name = "Lighting", Value = 400m, dateTime = DateTime.ParseExact("2024-11-12-10-30-20", "yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture)},
        };
    }
}
