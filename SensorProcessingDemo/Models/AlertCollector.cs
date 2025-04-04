﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SensorProcessingDemo.Models
{
    [Table("AlertCollector")]
    public class AlertCollector
    {
        [Key]        
        public int Id { get; set; }
        
        [ForeignKey(nameof(Sensor))]
        public int SensorId { get; set; }

        public virtual Sensor Sensor { get; set; }

        public AlertCollector(int sensorId)
        {
            this.SensorId = sensorId;
        }
    }
}