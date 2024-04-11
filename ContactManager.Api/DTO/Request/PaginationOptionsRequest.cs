using System.ComponentModel.DataAnnotations;

namespace ContactManager.Api.DTO.Request
{
    public class PaginationOptionsRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Limit { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Offset { get; set; }

        public string? OrderBy { get; set; }

        public OrderDirection? OrderDirection { get; set; }
    }

    public enum OrderDirection
    {
        ASC,
        DESC
    }
}