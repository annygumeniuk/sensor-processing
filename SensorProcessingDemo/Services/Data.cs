using SensorProcessingDemo.Models;
using System.Globalization;
namespace SensorProcessingDemo.Services
{
    public class Data
    {
        private static string format = "yyyy-MM-dd-HH-mm-ss";

        public List<Sensor> sensors = new List<Sensor>()
        { 
            new Sensor() { Id = 1, Name = "Temperature", Value = 20m,   dateTime = DateTime.ParseExact("2024-11-12-08-30-45", format, CultureInfo.InvariantCulture)},
            new Sensor() { Id = 2, Name = "Temperature", Value = 18.5m, dateTime = DateTime.ParseExact("2024-11-12-10-30-20", format, CultureInfo.InvariantCulture)},
            new Sensor() { Id = 7, Name = "Temperature", Value = 22m,   dateTime = DateTime.ParseExact("2024-11-12-11-30-20", format, CultureInfo.InvariantCulture)},

            new Sensor() { Id = 3, Name = "Humidity", Value = 35m, dateTime = DateTime.ParseExact("2024-11-12-08-30-45", format, CultureInfo.InvariantCulture)},
            new Sensor() { Id = 4, Name = "Humidity", Value = 55m, dateTime = DateTime.ParseExact("2024-11-12-10-30-20", format, CultureInfo.InvariantCulture)},
            new Sensor() { Id = 8, Name = "Humidity", Value = 44m, dateTime = DateTime.ParseExact("2024-11-12-11-30-20", format, CultureInfo.InvariantCulture)},

            new Sensor() { Id = 5, Name = "Lighting", Value = 500m, dateTime = DateTime.ParseExact("2024-11-12-08-30-45", format, CultureInfo.InvariantCulture)},
            new Sensor() { Id = 6, Name = "Lighting", Value = 400m, dateTime = DateTime.ParseExact("2024-11-12-10-30-20", format, CultureInfo.InvariantCulture)},
            new Sensor() { Id = 9, Name = "Lighting", Value = 350m, dateTime = DateTime.ParseExact("2024-11-12-11-30-20", format, CultureInfo.InvariantCulture)},
        };
    }
}
