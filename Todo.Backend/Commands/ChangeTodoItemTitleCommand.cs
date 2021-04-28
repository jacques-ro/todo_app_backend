using MediatR;
using System;

namespace Todo.Backend.Commands
{
    public class ChangeTodoItemTitleCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
    }    
}
