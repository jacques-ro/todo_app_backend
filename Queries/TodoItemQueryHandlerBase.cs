using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Todo.Backend.Models;
using Todo.Backend.Persistence.Context;

namespace Todo.Backend.Queries
{
    internal abstract class TodoItemQueryHandlerBase<TQuery, TResult> : IRequestHandler<TQuery, TResult>
        where TQuery : IRequest<TResult>
    {
        protected readonly TodoItemContext TodoItemContext;

        public TodoItemQueryHandlerBase(TodoItemContext todoItemContext)
        {
            TodoItemContext = todoItemContext;
        }

        public abstract Task<TResult> Handle(TQuery request, CancellationToken cancellationToken);

        protected TodoItem MapEntityOrReturnNullIfNotExisting(Persistence.Entities.TodoItem entity)
        {
            if(entity == null)
            {
                return null;
            }
            
            var item = new TodoItem();

            item.Id = entity.Id;
            item.IsCompleted = entity.IsCompleted;
            item.Title = entity.Title;

            return item;
        }
    }
}