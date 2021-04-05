using System;
using System.Collections.Generic;
using Todo.Backend.Models;

namespace Todo.Backend.Contract.Repository
{
    public interface ITodoRepository
    {
        void SaveAsync(TodoItem item);

        IEnumerable<TodoItem> GetAllItems();

        TodoItem GetItemById(Guid id);
    }
}