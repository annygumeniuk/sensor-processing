using System.ComponentModel.DataAnnotations;
using SensorProcessingDemo.Common;

namespace SensorProcessingDemo.Models
{
    public class Location
    {
        [Range(Common.Constants.LAT_MIN, Common.Constants.LAT_MAX, ErrorMessage= Common.Constants.LAT_ErrorMessage)]
        public double Latitude { get; set; }

        [Range(Common.Constants.LONG_MIN, Common.Constants.LONG_MAX, ErrorMessage = Common.Constants.LONG_ErrorMessage)]
        public double Longitude { get; set; }
    }
}
