using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Todo.Backend.Contract.Repository;

namespace Todo.Backend.Queries
{
    internal abstract class TodoItemQueryHandlerBase<TQuery, TResult> : IRequestHandler<TQuery, TResult>
        where TQuery : IRequest<TResult>
    {
        protected ITodoReadRepository Repository { get; }

        public TodoItemQueryHandlerBase(ITodoReadRepository repository)
        {
            Repository = repository;
        }

        public abstract Task<TResult> Handle(TQuery request, CancellationToken cancellationToken);
    }
}