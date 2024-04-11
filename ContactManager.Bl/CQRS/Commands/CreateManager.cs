using ContactManager.Persistance.Entities;
using ContactManager.Persistance.Reposetories;
using MediatR;

namespace ContactManager.Bl.CQRS.Commands
{
    public class CreateManager
    {
        public record Command(
            string? Name,
            bool? Married,
            string? PhoneNumber,
            DateTime? BirthDate,
            decimal Salary
            ) : IRequest<int>;

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly ManagerRepository _managerRepository;

            public Handler(ManagerRepository managerRepository)
            {
                _managerRepository = managerRepository;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var newManager = new ManagerEntity
                {
                    Name = request.Name,
                    Married = request.Married,
                    PhoneNumber = request.PhoneNumber,
                    BirthDate = request.BirthDate,
                    Salary = request.Salary
                };

                var managerId = await _managerRepository.CreateManager(newManager);

                return managerId;
            }
        }
    }
}