using Microsoft.EntityFrameworkCore;
using SensorProcessingDemo.Models;

namespace SensorProcessingDemo.Services
{
    public class MonitoringSystemContext : DbContext
    {
        public DbSet<User>           Users           { get; set; }
        public DbSet<Sensor>         Sensors         { get; set; }
        public DbSet<AlertCollector> AlertsCollector { get; set; }
        public DbSet<Monitoring>     Monitorings     { get; set; }

        public MonitoringSystemContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlertCollector>()
                .HasOne(ac => ac.Sensor)
                .WithOne(s => s.AlertCollector)
                .HasForeignKey<AlertCollector>(ac => ac.SensorId);
        }
    }    
}
