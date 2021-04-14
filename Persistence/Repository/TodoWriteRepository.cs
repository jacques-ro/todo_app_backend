using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
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
    public class TodoWriteRepository : ITodoWriteRepository
    {
        private readonly TodoItemContext _dbContext;
        public TodoWriteRepository(TodoItemContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(TodoItem item, CancellationToken cancellationToken)
        {
            var entity = new Persistence.Entities.TodoItem 
            {
                Id = item.Id,
                IsCompleted = item.IsCompleted,
                Title = item.Title
            };

            await _dbContext.TodoItems.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken); 
        }

        public async Task DeleteAsync(TodoItem item, CancellationToken cancellationToken)
        {
            var dbItem = await _dbContext.TodoItems.FirstOrDefaultAsync(i => i.Id == item.Id, cancellationToken);
            _dbContext.TodoItems.Remove(dbItem);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(TodoItem item, CancellationToken cancellationToken)
        {    
            var dbItem = await _dbContext.TodoItems.FirstOrDefaultAsync(i => i.Id == item.Id, cancellationToken);

            if(dbItem == null)
            {
                return;
            }

            dbItem.IsCompleted = item.IsCompleted;
            dbItem.Title = item.Title;

            await _dbContext.SaveChangesAsync(cancellationToken); 
        }
    }
}