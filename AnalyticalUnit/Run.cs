using AnalyticalUnit;
using AnalyticalUnit.Utils;
using DataGeneratorLibrary;

if (Toogles.BUILD_TABLE) TableBuilder.BuidTable();

if (Toogles.PARCE_SENSOR_CSV)
{    
    string filePath = Path.Combine(AppContext.BaseDirectory, "CsvStorage", "sensors_data.csv");
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

if (Toogles.WEATHER_FORECASTING)
{
    try {
        string filePath = Path.Combine(AppContext.BaseDirectory, "CsvStorage", "sensors_data.csv");
        string csvFilePath = filePath;

        // Create weather forecaster with default ARIMA parameters (p=2, d=1, q=2)
        var forecaster = new WeatherForecaster();

        // Generate forecast for next 24 hours
        var forecastResult = forecaster.ForecastWeather(csvFilePath, 24);

        // Display results
        Console.WriteLine("Weather Forecast for the Next 24 Hours:");
        Console.WriteLine("======================================");

        foreach (var parameter in forecastResult.Forecasts.Keys)
        {
            Console.WriteLine($"\n{parameter} Forecast (MSE: {forecastResult.MseValues[parameter]:F4}):");
            Console.WriteLine("------------------------------------------");

            foreach (var point in forecastResult.Forecasts[parameter])
            {
                Console.WriteLine($"{point.DateTime}: {point.Value:F2}");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
        }
    }
}