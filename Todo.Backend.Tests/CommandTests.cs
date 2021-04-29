using backend.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Todo.Backend.Commands;
using Todo.Backend.Commands.Exceptions;
using Todo.Backend.Contract.Repository;
using Todo.Backend.Exceptions;
using Todo.Backend.Models;
using Xunit;
using Xunit.Abstractions;

namespace Backend.Tests
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
    public class CommandTests
    {
        private readonly MockRepository _repo;

        public CommandTests()
        {
            _repo = new MockRepository();            
        }

        [Fact]
        public async void CreateTodoItemCommand_CreatesItem()
        {
            // act

            var createHandler = new CreateTodoItemCommandHandler(_repo);
            var itemId = await createHandler.Handle(new CreateTodoItemCommand { Title = "My Item" }, CancellationToken.None);            

            var item = await _repo.GetItemById(itemId, CancellationToken.None);

            // assert

            Assert.NotNull(item);
            Assert.False(item.IsCompleted);
            Assert.Equal(itemId, item.Id);
            Assert.Equal("My Item", item.Title);
        }

        [Fact]
        public async void CreateTodoItemCommand_WithoutValidTitle_Throws()
        {
            // act

            var createHandler = new CreateTodoItemCommandHandler(_repo);
            var exception = await Record.ExceptionAsync(async () => await createHandler.Handle(new CreateTodoItemCommand { Title = "" }, CancellationToken.None));                       

            // assert

            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public async void ChangeTodoItemTitleCommand_ChangesItemTitle()
        {
            // arrange

            var createHandler = new CreateTodoItemCommandHandler(_repo);
            var itemId = await createHandler.Handle(new CreateTodoItemCommand { Title = "My Item" }, CancellationToken.None);

            // act

            var handler = new ChangeTodoItemTitleCommandHandler(_repo, _repo);
            await handler.Handle(new ChangeTodoItemTitleCommand { Id = itemId, Title = "My Item 2" }, CancellationToken.None);

            var item = await _repo.GetItemById(itemId, CancellationToken.None);

            // assert

            Assert.Equal("My Item 2", item.Title);            
        }

        [Fact]
        public async void ChangeTodoItemTitleCommand_WithoutValidGuid_Throws()
        {
            // arrange

            var createHandler = new CreateTodoItemCommandHandler(_repo);
            var itemId = await createHandler.Handle(new CreateTodoItemCommand { Title = "My Item" }, CancellationToken.None);

            // act

            var handler = new ChangeTodoItemTitleCommandHandler(_repo, _repo);
            var exception = await Record.ExceptionAsync(async () => await handler.Handle(new ChangeTodoItemTitleCommand { Id = Guid.Empty, Title = "My Item 2" }, CancellationToken.None));

            // assert

            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public async void ChangeTodoItemTitleCommand_OnNonExistingItem_Throws()
        {
            // act

            var handler = new ChangeTodoItemTitleCommandHandler(_repo, _repo);
            var exception = await Record.ExceptionAsync(async () => await handler.Handle(new ChangeTodoItemTitleCommand { Id = Guid.NewGuid(), Title = "My Item 2" }, CancellationToken.None));

            // assert

            Assert.NotNull(exception);
            Assert.IsType<ItemNotFoundException>(exception);
        }

        [Fact]
        public async void ChangeTodoItemTitleCommand_WithoutValidTitle_Throws()
        {
            // arrange

            var createHandler = new CreateTodoItemCommandHandler(_repo);
            var itemId = await createHandler.Handle(new CreateTodoItemCommand { Title = "My Item" }, CancellationToken.None);

            // act

            var handler = new ChangeTodoItemTitleCommandHandler(_repo, _repo);
            var exception = await Record.ExceptionAsync(async () => await handler.Handle(new ChangeTodoItemTitleCommand { Id = itemId, Title = "My Item 2" }, CancellationToken.None));

            // assert

            Assert.NotNull(exception);
            Assert.IsType<CommandArgumentException>(exception);
        }

        [Fact]
        public async void TickOffTodoItemCommand_TicksOffTodoItem()
        {
            // arrange

            var createHandler = new CreateTodoItemCommandHandler(_repo);
            var itemId = await createHandler.Handle(new CreateTodoItemCommand { Title = "My Item" }, CancellationToken.None);

            // act

            var handler = new TickOffTodoItemCommandHandler(_repo, _repo);
            await handler.Handle(new TickOffTodoItemCommand { Id = itemId }, CancellationToken.None);

            var item = await _repo.GetItemById(itemId, CancellationToken.None);

            // assert

            Assert.True(item.IsCompleted);
        }

        [Fact]
        public async void DeleteTodoItemCommand_DeletesTodoItem()
        {
            // arrange

            var createHandler = new CreateTodoItemCommandHandler(_repo);
            var itemId = await createHandler.Handle(new CreateTodoItemCommand { Title = "My Item" }, CancellationToken.None);

            // act

            var handler = new DeleteTodoItemCommandHandler(_repo, _repo);
            await handler.Handle(new DeleteTodoItemCommand { Id = itemId }, CancellationToken.None);

            var item = await _repo.GetItemById(itemId, CancellationToken.None);

            // assert

            Assert.Null(item);
        }

        [Fact]
        public async void DeleteTodoItemCommand_WithoutValidGuid_Throws()
        {

            // act

            var handler = new DeleteTodoItemCommandHandler(_repo, _repo);
            var exception = await Record.ExceptionAsync(async () => await handler.Handle(new DeleteTodoItemCommand { Id = Guid.Empty }, CancellationToken.None));

            // assert

            Assert.NotNull(exception);
            Assert.IsType<CommandArgumentException>(exception);
        }

        [Fact]
        public async void DeleteTodoItemCommand_OnNonExistingItem_Throws()
        {

            // act

            var handler = new DeleteTodoItemCommandHandler(_repo, _repo);
            var exception = await Record.ExceptionAsync(async () => await handler.Handle(new DeleteTodoItemCommand { Id = Guid.NewGuid() }, CancellationToken.None));

            // assert

            Assert.NotNull(exception);
            Assert.IsType<ItemNotFoundException>(exception);
        }
    }
}
