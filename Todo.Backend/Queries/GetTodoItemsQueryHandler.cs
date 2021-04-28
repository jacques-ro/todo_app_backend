using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Todo.Backend.Contract.Repository;
using Todo.Backend.Models;

namespace Todo.Backend.Queries
{
    internal class GetTodoItemsQueryHandler : TodoItemQueryHandlerBase<GetTodoItemsQuery, IEnumerable<TodoItem>>
    {
        
        public GetTodoItemsQueryHandler(ITodoReadRepository repository) : base (repository)
        {}          

        public override async Task<IEnumerable<TodoItem>> Handle(GetTodoItemsQuery request, CancellationToken cancellationToken)
        {
            return await Repository.GetAllItems(cancellationToken);            
        }
    }
}