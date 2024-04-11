namespace ContactManager.Api.DTO.Response
{
    public record GetManagerByIdResponse(int Id,
                                        string? Name,
                                        bool? Married,
                                        string? PhoneNumber,
                                        DateTime? BirthDate,
                                        decimal Salary);
}