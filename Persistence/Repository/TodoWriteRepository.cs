using System;
using System.Collections.Generic;
using Todo.Backend.Contract.Repository;
using Todo.Backend.Models;

namespace Todo.Backend.Persistence.Repository
{
    /// <summary>
    /// SQL Read Repository for the todo persistence using EF Core
    /// </summary>
    public class TodoWriteRepository : ITodoWriteRepository
    {
        public void SaveAsync(TodoItem item)
        {
            
        }
    }
}