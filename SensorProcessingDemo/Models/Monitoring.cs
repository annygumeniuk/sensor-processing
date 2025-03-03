using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SensorProcessingDemo.Models
{
    [Table("Monitoring")]
    public class Monitoring
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime MonitoringStartedAt { get; set; }
        public DateTime? MonitoringStoppedAt { get; set; }
    }
}