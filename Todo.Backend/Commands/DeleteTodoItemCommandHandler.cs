using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Todo.Backend.Commands;
using Todo.Backend.Contract.Repository;

namespace backend.Commands
{
    internal class DeleteTodoItemCommandHandler : TodoItemCommandHandlerBase<DeleteTodoItemCommand, Unit>
    {
        // TODO: Currently, read and write persistence as well as models are equal, so using the read model is perfectly
        //       fine for the first iteration. However, commands should NEVER use read model persistence to rehydrate the
        //       model for change actions. Later, we need dedicated rehydration logic for the write model.
        private ITodoReadRepository _readRepository;

        public DeleteTodoItemCommandHandler(
            ITodoWriteRepository repository,
            ITodoReadRepository readRepository) : base(repository)
        {
            _readRepository = readRepository;
        }

        public async override Task<Unit> Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _readRepository.GetItemById(request.Id, cancellationToken);
            await Repository.DeleteAsync(item, cancellationToken);
            return Unit.Value;
        }
    }
}