using MediatR;
using System.Collections.Generic;
using Todo.Backend.Models;

namespace Todo.Backend.Queries
{
    internal class GetTodoItemsQuery : IRequest<IEnumerable<TodoItem>>
    {}
}