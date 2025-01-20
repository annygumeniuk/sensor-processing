using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SensorProcessingDemo.Models
{
    public class AlertCollector
    {
        [Key]
        [ForeignKey(nameof(Sensor))]
        public int SensorId { get; set; }

        public virtual Sensor? Sensor { get; set; }
    }
}