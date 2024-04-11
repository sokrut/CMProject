using ContactManager.Api.DTO.Models;
using ContactManager.Api.DTO.Request;
using ContactManager.Api.DTO.Response;
using ContactManager.Bl.CQRS.Commands;
using ContactManager.Bl.CQRS.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ManagerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetManagerByIdResponse>> GetManagerById([FromRoute] int id)
        {
            var result = await _mediator.Send(new GetManagerById.Query(id));

            return Ok(new GetManagerByIdResponse(result.Id, 
                                                 result.Name, 
                                                 result.Married, 
                                                 result.PhoneNumber, 
                                                 result.BirthDate, 
                                                 result.Salary));
        }

        [HttpGet]
        public async Task<ActionResult<GetManagersResponse>> GetManagers([FromQuery] ManagerFilterOptionsRequest filter, [FromQuery] PaginationOptionsRequest pagination)
        {
            var sortParameter = string.Concat(pagination.OrderBy, ":", pagination.OrderDirection.ToString());
            var result = await _mediator.Send(new GetManagers.Query(
                pagination.Offset,
                pagination.Limit,
                sortParameter,
                filter.Name,
                filter.PhoneNumber,
                filter.BirthDateFrom,
                filter.BirthDateTo,
                filter.SalaryFrom,
                filter.SalaryTo,
                filter.Married));

            var data = result.Managers?.Select(x => new ManagerBase()
            {
                Id = x.Id,
                Name = x.Name,
                Married = x.Married,
                BirthDate = x.BirthDate,
                PhoneNumber = x.PhoneNumber,
                Salary = x.Salary
            }).ToList();

            return Ok(new GetManagersResponse(data, result.Count));
        }

        [HttpGet]
        [Route("csv")]
        public async Task<FileResult> GetManagerCsv([FromQuery] ManagerFilterOptionsRequest filter)
        {
            var result = await _mediator.Send(new GetManagersCsv.Query(
                filter.Name,
                filter.PhoneNumber,
                filter.BirthDateFrom,
                filter.BirthDateTo,
                filter.SalaryFrom,
                filter.SalaryTo,
                filter.Married));

            return File(result, "text/csv", $"manager.csv");
        }

        [HttpPost]
        public async Task<ActionResult<CreateManagerResponse>> CreateManager([FromBody] CreateManagerRequest request)
        {
            var result = await _mediator.Send(
                new CreateManager.Command(
                    request.Name,
                    request.Married,
                    request.PhoneNumber,
                    request.BirthDate,
                    request.Salary));

            return Ok(new CreateManagerResponse(result));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<int>> UpdateManager([FromRoute] int id, [FromBody] UpdateManagerRequest request)
        {
            var result = await _mediator.Send(new UpdateManager.Command(
                id, 
                request.Name, 
                request.Married, 
                request.PhoneNumber,
                request.BirthDate, 
                request.Salary));

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> DeleteManager([FromRoute] int id)
        {
            var result = await _mediator.Send(new DeleteManager.Command(id));

            return Ok(result);
        }
    }
}
