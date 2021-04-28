using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Todo.Backend.Commands;
using Todo.Backend.Contract.Repository;

namespace Todo.Backend.Commands
{
    internal class TickOffTodoItemCommandHandler : TodoItemCommandHandlerBase<TickOffTodoItemCommand, Unit>
    {
        
        // TODO: Currently, read and write persistence as well as models are equal, so using the read model is perfectly
        //       fine for the first iteration. However, commands should NEVER use read model persistence to rehydrate the
        //       model for change actions. Later, we need dedicated rehydration logic for the write model.
        private ITodoReadRepository _readRepository;

        public TickOffTodoItemCommandHandler(
            ITodoWriteRepository repository,
            ITodoReadRepository readRepository) : base(repository)
        {
            _readRepository = readRepository;
        }

        public override async Task<Unit> Handle(TickOffTodoItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _readRepository.GetItemById(request.Id, cancellationToken);

            if(item == null)
            {
                throw new InvalidOperationException($"There is no todo item with id {request.Id}");
            }

            item.IsCompleted = true;
            await Repository.UpdateAsync(item, cancellationToken);

            return Unit.Value;
        }
    }
}