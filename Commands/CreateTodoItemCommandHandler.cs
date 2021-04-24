using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Todo.Backend.Contract.Repository;
using Todo.Backend.Models;
using Todo.Backend.Persistence.Context;

namespace Todo.Backend.Commands
{
    internal class CreateTodoItemCommandHandler : TodoItemCommandHandlerBase<CreateTodoItemCommand, Guid>
    {
        public CreateTodoItemCommandHandler(
            ITodoWriteRepository repository) : base(repository)
        {}

        public override async Task<Guid> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
        {            

            var id = Guid.NewGuid();

            var todoItem = new TodoItem {
                Id = id,
                Title = request.Title,
                IsCompleted = false                
            };

            await Repository.AddAsync(todoItem, cancellationToken);

            return id;
        }
    }
}