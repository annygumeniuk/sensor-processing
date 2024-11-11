using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace SensorProcessingDemo.Attributes
{
    public class DateTimeFormatAttribute : ValidationAttribute
    {
        private readonly string _dateFormat;

        public DateTimeFormatAttribute(string dateFormat)
        {
            _dateFormat = dateFormat;
            ErrorMessage = $"The date must be in the format {_dateFormat}.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success; // or handle null as invalid, based on your requirements
            }

            if (DateTime.TryParseExact(value.ToString(), _dateFormat,
                                       CultureInfo.InvariantCulture,
                                       DateTimeStyles.None, out _))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(ErrorMessage);
            }
        }
    }
}
