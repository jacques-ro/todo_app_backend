using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Todo.Backend.Persistence.Context;

namespace Todo.Backend.Commands
{
    internal class ChangeTodoItemTitleCommandHandler : TodoItemCommandHandlerBase<ChangeTodoItemTitleCommand, Unit>
    {

        public ChangeTodoItemTitleCommandHandler(TodoItemContext todoItemContext) : base(todoItemContext)
        {}

        public override async Task<Unit> Handle(ChangeTodoItemTitleCommand request, CancellationToken cancellationToken)
        {
            var item = await TodoItemContext.TodoItems.FirstOrDefaultAsync(i => i.Id == request.Id);
            item.Title = request.Title;
            await TodoItemContext.SaveChangesAsync();

            return Unit.Value;
        }
    }
}