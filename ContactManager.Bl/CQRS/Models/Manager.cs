namespace ContactManager.Bl.CQRS.Models
{
    public class Manager
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        //TODO: Why nullable?
        public bool? Married { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime? BirthDate { get; set; }

        public decimal Salary { get; set; }
    }
}