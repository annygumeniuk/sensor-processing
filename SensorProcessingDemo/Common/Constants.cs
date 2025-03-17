using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SensorProcessingDemo.Common
{
    public static class Constants
    {                
        // Enum to store sensors names
        public enum SENSORNAME { Temperature, Humidity, Lighting };

        // Interval to update plots
        public const int UpdateIntervalSeconds = 2;

        // In °C 
        public const decimal TEMP_MIN   = 18.0m;
        public const decimal TEMP_MAX   = 22.5m; 

        // In %
        public const decimal HUM_MIN    = 30.0m;
        public const decimal HUM_MAX    = 60.0m;

        // In lx
        public const decimal LIGHT_MIN  = 300m;
        public const decimal LIGHT_MAX  = 1000m;

        // Latitude and longitude ranges
        public const int LAT_MAX =  90;
        public const int LAT_MIN = -90;

        public const int LONG_MAX =  180;
        public const int LONG_MIN = -180;

        // Default error messages
        public const string LAT_ErrorMessage = $"Latitude should be in range from -90 to 90";
        public const string LONG_ErrorMessage = $"Longitude  should be in range from -180 to 180";

        // Coordinates of Kyiv
        public const double LAT_KYIV  = 50.4504;
        public const double LONG_KYIV = 30.5245;

        // [NOT USED FOR NOW] Dictionary of default coordinates
        public static Dictionary<string, List<double>> defaultPlaces = new Dictionary<string, List<double>>()
        {
            {"Kyiv",  new List<double>() { 50.45, 30.52}},
            {"Odesa", new List<double>() { 46.47, 30.73 }},
            {"Lviv",  new List<double>() { 49.83, 24.02 }}
        };
    }
}