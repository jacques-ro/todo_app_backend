using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Todo.Backend.Contract.Repository;
using Todo.Backend.Persistence.Context;

namespace Todo.Backend.Commands
{
    internal abstract class TodoItemCommandHandlerBase<TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : IRequest<TResponse>
    {
        protected readonly ITodoWriteRepository Repository;

        public TodoItemCommandHandlerBase(ITodoWriteRepository repository)
        {
            Repository = repository;
        }

        public abstract Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken);
    }
}