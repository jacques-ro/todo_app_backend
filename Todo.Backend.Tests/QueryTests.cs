using System;
using System.Threading;
using Todo.Backend.Models;
using Todo.Backend.Queries;
using Todo.Backend.Tests.Mocks;
using Xunit;

namespace Todo.Backend.Tests
{
    public class QueryTests
    {
        private readonly MockRepository _repo;

        public QueryTests()
        {
            _repo = new MockRepository();
        }       

        [Fact]
        public async void GetTodoItemByIdQuery_ReturnsCorrectItem()
        {
            // arrange

            var itemId = Guid.NewGuid();
            await _repo.AddAsync(new TodoItem { Id = itemId, IsCompleted = false, Title = "My Item" }, CancellationToken.None);

            // act

            var queryHandler = new GetTodoItemByIdQueryHandler(_repo);
            var item = await queryHandler.Handle(new GetTodoItemByIdQuery { Id = itemId }, CancellationToken.None);

            // assert

            Assert.NotNull(item);
            Assert.Equal(itemId, item.Id);
        }

        [Fact]
        public async void GetTodoItemByIdQuery_WithNonExistingItem_ReturnsNull()
        {
            // act

            var queryHandler = new GetTodoItemByIdQueryHandler(_repo);
            var item = await queryHandler.Handle(new GetTodoItemByIdQuery { Id = Guid.NewGuid() }, CancellationToken.None);

            // assert

            Assert.Null(item);
        }
        
        [Fact]
        public async void GetTodoItemsQuery_ReturnsAllItems()
        {
        // arrange

            var itemId1 = Guid.NewGuid();
            var itemId2 = Guid.NewGuid();
            await _repo.AddAsync(new TodoItem { Id = itemId1, IsCompleted = false, Title = "My Item" }, CancellationToken.None);
            await _repo.AddAsync(new TodoItem { Id = itemId2, IsCompleted = false, Title = "My Item 2" }, CancellationToken.None);

            // act

            var queryHandler = new GetTodoItemsQueryHandler(_repo);
            var items = await queryHandler.Handle(new GetTodoItemsQuery(), CancellationToken.None);

            // assert
            
            Assert.NotNull(items);
            Assert.Collection(items,
                item => {
                    Assert.Equal(itemId1, item.Id);
                },
                item => {
                    Assert.Equal(itemId2, item.Id);
                }
            );
        }

        [Fact]
        public async void GetTodoItemsQuery_ReturnsEmptyList()
        {
            // act

            var queryHandler = new GetTodoItemsQueryHandler(_repo);
            var items = await queryHandler.Handle(new GetTodoItemsQuery(), CancellationToken.None);

            // assert
            
            Assert.NotNull(items);
            Assert.Empty(items);
        }
    }
}