using ContactManager.Bl.CQRS.Models;
using ContactManager.Persistance.Models;
using ContactManager.Persistance.Reposetories;
using CsvHelper;
using MediatR;
using System.Globalization;
using System.Text;
using static ContactManager.Bl.CQRS.Queries.GetManagers;

namespace ContactManager.Bl.CQRS.Queries
{
    public class GetManagersCsv
    {
        public record Query(
            string? Name,
            string? PhoneNumber,
            DateTime? BirthDateFrom,
            DateTime? BirthDateTo,
            decimal? SalaryFrom,
            decimal? SalaryTo,
            bool? Married) : IRequest<byte[]>;

        public class Handler : IRequestHandler<Query, byte[]>
        {
            private readonly ManagerRepository _managerRepository;

            public Handler(ManagerRepository managerRepository)
            {
                _managerRepository = managerRepository;
            }

            public async Task<byte[]> Handle(Query request, CancellationToken cancellationToken)
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

                var managers = await _managerRepository.GetManagersInfoForCsv(filter);
                var managersCsv = managers.Select(x => new ManagerCsv()
                {
                    Name = x.Name,
                    PhoneNumber = x.PhoneNumber,
                    BirthDate = x.BirthDate,
                    Married = x.Married,
                    Salary = x.Salary
                });

                using var ms = new MemoryStream();
                using var sw = new StreamWriter(ms, new UTF8Encoding(true));
                using (var cw = new CsvWriter(sw, CultureInfo.InvariantCulture))
                {
                    cw.WriteRecords(managersCsv);
                }

                return ms.ToArray();
            }
        }
    }
}
