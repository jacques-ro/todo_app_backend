using backend.Commands;
using System;
using System.Threading;
using Todo.Backend.Commands;
using Todo.Backend.Commands.Exceptions;
using Todo.Backend.Exceptions;
using Todo.Backend.Tests.Mocks;
using Xunit;

namespace Backend.Tests
{
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
            Assert.IsType<CommandArgumentException>(exception);
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
            Assert.IsType<CommandArgumentException>(exception);
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
            var exception = await Record.ExceptionAsync(async () => await handler.Handle(new ChangeTodoItemTitleCommand { Id = itemId, Title = "" }, CancellationToken.None));

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
