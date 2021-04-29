using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Todo.Backend.Contract.Repository;
using Todo.Backend.Models;

namespace Todo.Backend.Tests.Mocks
{
    #region Mock Classes

    internal class MockRepository : ITodoWriteRepository, ITodoReadRepository
    {
        private readonly List<TodoItem> _items;

        public MockRepository()
        {
            _items = new List<TodoItem>();
        }
        
        public async Task AddAsync(TodoItem item, CancellationToken cancellationToken)
        {
            await Task.Run(() => _items.Add(item), cancellationToken);
        }

        public async Task DeleteAsync(TodoItem item, CancellationToken cancellationToken)
        {
            await Task.Run(() => _items.RemoveAll(i => i.Id == item.Id), cancellationToken);
        }

        public async Task UpdateAsync(TodoItem item, CancellationToken cancellationToken)
        {
            await Task.Run(() => {
                _items.RemoveAll(i => i.Id == item.Id);
                _items.Add(item);
            }, cancellationToken);
        }

        public async Task<IEnumerable<TodoItem>> GetAllItems(CancellationToken cancellationToken)
        {
            return await Task.Run(() => _items, cancellationToken);
        }

        public async Task<TodoItem> GetItemById(Guid id, CancellationToken cancellationToken)
        {
            return await Task.Run(() => _items.FirstOrDefault(i => i.Id == id), cancellationToken);
        }        
    }

    #endregion
}