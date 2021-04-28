using MediatR;
using System;
using Todo.Backend.Models;

namespace Todo.Backend.Queries
{
    internal class GetTodoItemByIdQuery : IRequest<TodoItem>
    {
        public Guid Id { get; set; }
    }
}