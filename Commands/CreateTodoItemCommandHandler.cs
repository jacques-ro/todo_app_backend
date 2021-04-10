using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Todo.Backend.Persistence.Context;

namespace Todo.Backend.Commands
{
    internal class CreateTodoItemCommandHandler : TodoItemCommandHandlerBase<CreateTodoItemCommand, Guid>
    {

        public CreateTodoItemCommandHandler(TodoItemContext todoItemContext) : base(todoItemContext)
        {}

        public override async Task<Guid> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
        {
            var entity = new Todo.Backend.Persistence.Entities.TodoItem();

            var id = Guid.NewGuid();

            entity.Id = id;
            entity.Title = request.Title;

            await TodoItemContext.TodoItems.AddAsync(entity, cancellationToken);
            await TodoItemContext.SaveChangesAsync();

            return id;
        }
    }
}