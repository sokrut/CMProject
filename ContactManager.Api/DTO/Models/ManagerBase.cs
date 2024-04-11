namespace ContactManager.Api.DTO.Models
{
    public class ManagerBase
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public bool? Married { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime? BirthDate { get; set; }

        public decimal Salary { get; set; }
    }
}