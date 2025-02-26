using Microsoft.EntityFrameworkCore;
using SensorProcessingDemo.Attributes;
using SensorProcessingDemo.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SensorProcessingDemo.Models
{
    [Table("Sensor")]
    public class Sensor
    {
        [Key]
        public int     Id    { get; set; }
        
        public int UserId { get; set; }
        public string  Name  { get; set; }
                
        public float Value { get; set; }        
        public DateTime dateTime { get; set; }
        
        public virtual AlertCollector? AlertCollector { get; set; }
        public virtual User? User { get; set; }

        public Sensor(int id, string name, float value, DateTime time)
        { 
            Id = id;
            Name = name;
            Value = value;
            dateTime = time;
        }

        public Sensor() { }   
    }
}