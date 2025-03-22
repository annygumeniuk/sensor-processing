using Microsoft.ML.Data;
namespace DataGeneratorLibrary
{
    public class WeatherData
    {
        [LoadColumn(0)] public float Temperature { get; set; }
        [LoadColumn(1)] public float Humidity    { get; set; }
        [LoadColumn(2)] public float Pressure    { get; set; }
        [LoadColumn(3)] public float Visibility  { get; set; }
    }
}
