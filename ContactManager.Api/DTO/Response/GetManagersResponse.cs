using ContactManager.Api.DTO.Models;

namespace ContactManager.Api.DTO.Response
{
    public record GetManagersResponse(IReadOnlyCollection<ManagerBase>? Data, int TotalCount);
}