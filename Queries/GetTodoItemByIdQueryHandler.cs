using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Todo.Backend.Contract.Repository;
using Todo.Backend.Models;
using Todo.Backend.Persistence.Context;

namespace Todo.Backend.Queries
{
    internal class GetTodoItemByIdQueryHandler : TodoItemQueryHandlerBase<GetTodoItemByIdQuery, TodoItem>
    {
        public GetTodoItemByIdQueryHandler(ITodoReadRepository repository) : base (repository)
        {}

        public override async Task<TodoItem> Handle(GetTodoItemByIdQuery request, CancellationToken cancellationToken)
        {
            return await Repository.GetItemById(request.Id);            
        }
    }
}