using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SensorProcessingDemo.Common
{
    public static class Constants
    {                        
        // Interval to update plots
        public const int UpdateIntervalSeconds = 2;

        // Temperature In °C 
        public const decimal TEMP_MIN   = -60.0m;
        public const decimal TEMP_MAX   =  50.0m;
        
        public const decimal ACCURACY_TEMP = 1.0m; // in °C 

        // Humidity In %
        public const decimal HUM_MIN    = 30.0m;
        public const decimal HUM_MAX    = 100.0m;

        public const decimal ACCURACY_HUM_IF_POSITIVE_TEMP = 5.0m; // in %
        public const decimal ACCURACY_HUM_IF_NEGATIVE_TEMP = 5.0m; // in %

        // Visibility In m
        // [Note] this if visible is in other ranges the accuracy should be changed.
        // All ranges: [20, 150], [150, 250], [250, 2000]
        public const decimal VIS_MIN  = 20m;
        public const decimal VIS_MAX  = 150m;

        public const decimal ACCURACY_VIS = 20.0m; // in %

        // Atmospheric Pressure in hectopascal hPa
        public const decimal ATM_PRESS_MIN = 600.0m;
        public const decimal ATM_PRESS_MAX = 1080.0m;

        public const decimal ACCURACY_ATM_PRESS = 0.05m; // in hPa


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