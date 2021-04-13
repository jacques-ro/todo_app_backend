using System;
using System.Collections.Generic;
using Todo.Backend.Models;

namespace Todo.Backend.Contract.Repository
{
    /// <summary>
    /// A write only repository to write todo items into a persistence storage
    /// </summary>
    public interface ITodoWriteRepository
    {
        void SaveAsync(TodoItem item);
    }
}