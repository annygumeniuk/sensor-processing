using ConsoleTables;

var table = new ConsoleTable("Date", "Temperature (°C)", "Humidity (%)");
table.AddRow("2025-03-17", 12.5, 80)
     .AddRow("2025-03-18", 14.2, 75)
     .AddRow("2025-03-19", 10.8, 85);

table.Write();
Console.ReadLine();
