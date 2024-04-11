using ContactManager.Bl.CQRS.Models;
using ContactManager.Persistance.Reposetories;
using MediatR;

namespace ContactManager.Bl.CQRS.Queries
{
    public class GetManagerById
    {
        public record Query(int ManagerId) : IRequest<Manager>;

        public class Handler : IRequestHandler<Query, Manager>
        {
            private readonly ManagerRepository _managerRepository;

            public Handler(ManagerRepository managerRepository)
            {
                _managerRepository = managerRepository;
            }

            public async Task<Manager> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _managerRepository.GetManagerById(request.ManagerId);

                return new Manager
                {
                    Id = user.Id,
                    Name = user.Name,
                    Married = user.Married,
                    PhoneNumber = user.PhoneNumber,
                    BirthDate = user.BirthDate,
                    Salary = user.Salary
                };
            }
        }
    }
}
