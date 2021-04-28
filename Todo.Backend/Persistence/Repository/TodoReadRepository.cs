using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Todo.Backend.Contract.Repository;
using Todo.Backend.Models;
using Todo.Backend.Persistence.Context;

namespace Todo.Backend.Persistence.Repository
{
    /// <summary>
    /// SQL Read Repository for the todo persistence using EF Core
    /// </summary>
    public class TodoReadRepository : ITodoReadRepository
    {
        private readonly TodoItemContext _dbContext;

        public TodoReadRepository(TodoItemContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TodoItem>> GetAllItems(CancellationToken cancellationToken)
        {
            var items = await _dbContext.TodoItems.ToListAsync(cancellationToken);
            return items.Select(i => mapEntityOrReturnNullIfNotExisting(i));
        }

        public async Task<TodoItem> GetItemById(Guid id, CancellationToken cancellationToken)
        {
            var item = await _dbContext.TodoItems.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
            return mapEntityOrReturnNullIfNotExisting(item);
        }

        private TodoItem mapEntityOrReturnNullIfNotExisting(Persistence.Entities.TodoItem entity)
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