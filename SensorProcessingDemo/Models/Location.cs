using System.ComponentModel.DataAnnotations;
using SensorProcessingDemo.Common;

namespace SensorProcessingDemo.Models
{
    public class Location
    {
        [Range(Constants.LAT_MIN,  Constants.LAT_MAX, ErrorMessage= Constants.LAT_ErrorMessage)]
        public double? Latitude { get; set; }

        [Range(Constants.LONG_MIN, Constants.LONG_MAX, ErrorMessage = Constants.LONG_ErrorMessage)]
        public double? Longitude { get; set; }
    }
}
