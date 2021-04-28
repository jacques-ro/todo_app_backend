using MediatR;
using System;

namespace Todo.Backend.Commands
{
    public class TickOffTodoItemCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }      
    }
}