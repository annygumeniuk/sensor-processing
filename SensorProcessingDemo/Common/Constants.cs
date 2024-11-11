namespace SensorProcessingDemo.Common
{
    public static class Constants
    {
        // LET`S IMAGINE ALL THIS CONSTS ARE FOR OPERATION ROOM

        public const string TEMPERATURE = "Temperature";
        public const string HUMIDINY    = "Humidity";
        public const string LIGHTING    = "Lighting";

        // In °C 
        public const decimal TEMP_MIN   = 18.0m;
        public const decimal TEMP_MAX   = 22.5m; 

        // In %
        public const decimal HUM_MIN    = 30.0m;
        public const decimal HUM_MAX    = 60.0m;

        // In lx
        public const decimal LIGHT_MIN  = 300m;
        public const decimal LIGHT_MAX  = 1000m;
    }
}
