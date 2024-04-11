using System.ComponentModel.DataAnnotations;

namespace ContactManager.Api.DTO.Request
{
    public class UpdateManagerRequest
    {
        [Required]
        [MaxLength(256)]
        public string? Name { get; set; }

        [Required]
        public bool Married { get; set; }

        [Required]
        [RegularExpression(@"^(38)?\d{10}$", ErrorMessage = "The phone number is not valid.")]
        public string? PhoneNumber { get; set; }

        [Required]
        public DateTime? BirthDate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Salary { get; set; }
    }
}