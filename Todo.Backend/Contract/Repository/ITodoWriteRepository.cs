using System.Threading;
using System.Threading.Tasks;
using Todo.Backend.Models;

namespace Todo.Backend.Contract.Repository
{
    /// <summary>
    /// A write only repository to write todo items into a persistence storage
    /// </summary>
    public interface ITodoWriteRepository
    {
        Task AddAsync(TodoItem item, CancellationToken cancellationToken);
        Task UpdateAsync(TodoItem item, CancellationToken cancellationToken);

        Task DeleteAsync(TodoItem item, CancellationToken cancellationToken);
    }
}