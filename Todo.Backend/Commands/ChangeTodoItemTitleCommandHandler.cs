using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Todo.Backend.Contract.Repository;
using Todo.Backend.Exceptions;

namespace Todo.Backend.Commands
{
    internal class ChangeTodoItemTitleCommandHandler : TodoItemCommandHandlerBase<ChangeTodoItemTitleCommand, Unit>
    {
        // TODO: Currently, read and write persistence as well as models are equal, so using the read model is perfectly
        //       fine for the first iteration. However, commands should NEVER use read model persistence to rehydrate the
        //       model for change actions. Later, we need dedicated rehydration logic for the write model.
        private readonly ITodoReadRepository _readRepository;

        public ChangeTodoItemTitleCommandHandler(
            ITodoWriteRepository repository,
            ITodoReadRepository readRepository) : base(repository)
        {
            _readRepository = readRepository;
        }

        public override async Task<Unit> Handle(ChangeTodoItemTitleCommand request, CancellationToken cancellationToken)
        {
            AssertValidGuid(request.Id);
            AssertValidTitle(request.Title);

            var item = await _readRepository.GetItemById(request.Id, cancellationToken);

            if(item == null)
            {
                throw new ItemNotFoundException(request.Id);
            }

            item.Title = request.Title;
            await Repository.UpdateAsync(item, cancellationToken);

            return Unit.Value;
        }
    }
}