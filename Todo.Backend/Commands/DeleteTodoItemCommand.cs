using MediatR;
using System;

namespace Todo.Backend.Commands
{
    public class DeleteTodoItemCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }      
    }
}