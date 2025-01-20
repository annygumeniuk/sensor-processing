using System.ComponentModel.DataAnnotations;

namespace SensorProcessingDemo.Models
{
    public class Monitoring
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime MonitoringStartedAt { get; set; }
        public DateTime MonitoringStoppedAt { get; set; }
    }
}