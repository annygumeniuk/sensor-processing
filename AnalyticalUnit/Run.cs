using AnalyticalUnit;
using AnalyticalUnit.Utils;

if (Toogles.BUILD_TABLE) TableBuilder.BuidTable();

if (Toogles.PARCE_SENSOR_CSV)
{
    string filePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "CsvStorage", "sensors_data.csv");      
    var sensorDataList = CsvSensorParser.ParseCsv(filePath);
   
    if (Toogles.DISPLAY_PARCED_SENSOR_DATA) CsvSensorParser.DisplayParsedDataInConsole(sensorDataList);    
}