using AnalyticalUnit;
using AnalyticalUnit.Utils;
using DataGeneratorLibrary;

if (Toogles.BUILD_TABLE) TableBuilder.BuidTable();

if (Toogles.PARCE_SENSOR_CSV)
{
    string filePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "CsvStorage", "sensors_data.csv");
    var sensorDataList = CsvSensorParser.ParseCsv(filePath);

    var temperatureData = sensorDataList
    .Where(d => d.Name == "Temperature")
    .OrderBy(d => d.DateTime)
    .Select(d => (double)d.Value)
    .ToList();

    if (Toogles.DISPLAY_PARCED_SENSOR_DATA)
    {
        CsvSensorParser.DisplayParsedDataInConsole(sensorDataList);
    }

    if (Toogles.FORCASTING_TEMP)
    {
        var forecastedValues = Forecasting.MovingAverageForecast(temperatureData, windowSize: 3);
        
        foreach (var item in forecastedValues)
        {
            Console.Write(item + "\n");
        }

        var mse = MeanSquaredError.CalculateMSE(temperatureData.Skip(temperatureData.Count - forecastedValues.Count).ToList(), forecastedValues);

        Console.WriteLine($"MSE: {mse}");
    }   
}

if (Toogles.DATA_GENERATOR)
{
    WeatherModel model = new WeatherModel();
    model.Generate();    
}