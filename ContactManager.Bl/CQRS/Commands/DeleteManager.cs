using ContactManager.Persistance.Reposetories;
using MediatR;

namespace ContactManager.Bl.CQRS.Commands
{
    public class DeleteManager
    {
        public record Command(int Id) : IRequest<int>;

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly ManagerRepository _managerRepository;

            public Handler(ManagerRepository managerRepository)
            {
                _managerRepository = managerRepository;
            }
            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                return await _managerRepository.DeleteManager(request.Id);
            }
        }
    }
}