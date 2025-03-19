using AnalyticalUnit.ParsingModels;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticalUnit.Utils
{
    public class CsvSensorParser
    {
        public static List<SensorParsingModel> ParseCsv(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true // Skips the first row (header)
            });

            return csv.GetRecords<SensorParsingModel>().ToList();
        }

        public static void DisplayParsedDataInConsole(List<SensorParsingModel> list)
        {
            foreach (var data in list)
            {
                Console.WriteLine($"Name: {data.Name}, Value: {data.Value}, DateTime: {data.DateTime}");
            }
        }
    }
}