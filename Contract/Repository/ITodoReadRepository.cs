using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Backend.Models;

namespace Todo.Backend.Contract.Repository
{
    /// <summary>
    /// A read only repository to read todo items from the peristence storage
    /// </summary>
    public interface ITodoReadRepository
    {        
        Task<IEnumerable<TodoItem>> GetAllItems();

        Task<TodoItem> GetItemById(Guid id);
    }
}