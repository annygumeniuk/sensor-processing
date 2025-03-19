using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SensorProcessingDemo.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "This field is required")]
        public string LastName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "This field is required")]
        public string Email { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "The minimun leght is 6")]
        [Required(ErrorMessage = "This field is required")]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } = "User";
    }
}