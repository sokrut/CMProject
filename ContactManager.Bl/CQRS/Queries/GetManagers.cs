using ContactManager.Bl.CQRS.Models;
using ContactManager.Persistance.Models;
using ContactManager.Persistance.Reposetories;
using MediatR;

namespace ContactManager.Bl.CQRS.Queries
{
    public class GetManagers
    {
        public record Query(
            int Offset, 
            int Limit,
            string? Sort, 
            string? Name, 
            string? PhoneNumber, 
            DateTime? BirthDateFrom, 
            DateTime? BirthDateTo, 
            decimal? SalaryFrom, 
            decimal? SalaryTo, 
            bool? Married) : IRequest<Data>;

        public class Handler : IRequestHandler<Query, Data>
        {
            private readonly ManagerRepository _managerRepository;

            public Handler(ManagerRepository managerRepository)
            {
                _managerRepository = managerRepository;
            }

            public async Task<Data> Handle(Query request, CancellationToken cancellationToken)
            {
                var filter = new ManagerFilterOptions
                {
                    Name = request.Name,
                    PhoneNumber = request.PhoneNumber,
                    BirthDateFrom = request.BirthDateFrom,
                    BirthDateTo = request.BirthDateTo,
                    Married = request.Married,
                    SalaryFrom = request.SalaryFrom,
                    SalaryTo = request.SalaryTo
                };

                var managers = await _managerRepository.GetManagers(filter, request.Limit, request.Offset, request.Sort);
                var count = await _managerRepository.GetManagerCount(filter);

                return new Data
                {
                    Count = count,
                    Managers = managers.Select(x => new Manager()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Married = x.Married,
                        PhoneNumber = x.PhoneNumber,
                        BirthDate = x.BirthDate,
                        Salary = x.Salary
                    }).ToList()
                };
            }
        }

        public class Data
        {
            public IList<Manager>? Managers { get; set; }
            public int Count { get; set; }
        }
    }
}
