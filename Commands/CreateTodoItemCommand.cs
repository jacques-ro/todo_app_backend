using MediatR;
using System;

namespace Todo.Backend.Commands
{
    public class CreateTodoItemCommand : IRequest<Guid>
    {
        public string Title { get; set; }
    }
}
