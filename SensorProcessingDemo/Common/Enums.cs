namespace SensorProcessingDemo.Common
{
    public class Enums
    {
        // To store sort directions
        public enum SortDirection
        {
            Ascending,
            Descending,
        }

        // To store sensors names
        public enum SENSORNAME 
        { 
            Temperature, 
            Humidity,
            Visibility,
            AtmosphericPressure
        };

        // [Not used for now] To store user roles
        public enum ROLES
        { 
            User,
            Admin
        };
    }
}
