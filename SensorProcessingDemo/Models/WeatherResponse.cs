namespace SensorProcessingDemo.Models
{
    public class WeatherResponse
    {
        public string   Name  { get; set; }
        public MainData Main  { get; set; }
        public int Visibility { get; set; }
    }

    public class MainData
    {
        public double Temp  { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
    }

}
