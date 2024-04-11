namespace ContactManager.Api.DTO.Request
{
    public class ManagerFilterOptionsRequest
    {
        public string? Name { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime? BirthDateFrom { get; set; }

        public DateTime? BirthDateTo { get; set; }

        public decimal? SalaryFrom { get; set; }

        public decimal? SalaryTo { get; set; }

        public bool? Married { get; set; }
    }
}