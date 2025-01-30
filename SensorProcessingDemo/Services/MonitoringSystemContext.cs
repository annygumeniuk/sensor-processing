using Microsoft.EntityFrameworkCore;
using SensorProcessingDemo.Models;

namespace SensorProcessingDemo.Services
{
    public class MonitoringSystemContext : DbContext
    {
        public DbSet<User>           User           { get; set; }
        public DbSet<Sensor>         Sensor         { get; set; }
        public DbSet<AlertCollector> AlertCollector { get; set; }
        public DbSet<Monitoring>     Monitoring     { get; set; }

        public MonitoringSystemContext(DbContextOptions options) : base(options)
        {
        }
    }    
}
