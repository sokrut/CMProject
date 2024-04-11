namespace ContactManager.Bl.CQRS.Models
{
    public class ManagerCsv
    { 
        public string? Name { get; set; }

        public bool? Married { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime? BirthDate { get; set; }

        public decimal Salary { get; set; }
    }
}