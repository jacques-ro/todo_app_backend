using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Todo.Backend.Models;
using Todo.Backend.Persistence.Context;

namespace Todo.Backend.Queries
{
    internal class GetTodoItemByIdQueryHandler : TodoItemQueryHandlerBase<GetTodoItemByIdQuery, TodoItem>
    {
        public GetTodoItemByIdQueryHandler(TodoItemContext todoItemContext) : base (todoItemContext)
        {}

        public override async Task<TodoItem> Handle(GetTodoItemByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await TodoItemContext.TodoItems.FirstOrDefaultAsync(i => i.Id == request.Id);
            return MapEntityOrReturnNullIfNotExisting(item);
        }
    }
}