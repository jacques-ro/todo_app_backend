
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Backend.DTOs;
using Todo.Backend.Models;
using Todo.Backend.Persistence.Context;

namespace Todo.Backend.Controllers
{

    [Route("/api/todos/")]
    [ApiController]
    public class TodoItemController : ControllerBase
    {
        private readonly TodoItemContext _todoItemContext;

        public TodoItemController(TodoItemContext todoItemContext)
        {
            _todoItemContext = todoItemContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodoItem([FromBody] CreateTodoItemCommand createItemCommand, CancellationToken cancellationToken)
        {
            var entity = mapCreateCommand(createItemCommand);
            await _todoItemContext.TodoItems.AddAsync(entity, cancellationToken);
            await _todoItemContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItemById), new { todoItemId = entity.Id }, entity.Id);
        }
    
        [HttpPut("{todoItemId}")]
        public async Task<IActionResult> ChangeTodoItemTitle([FromRoute] Guid todoItemId, [FromBody] ChangeTodoItemTitleCommand changeTitleCommand, CancellationToken cancellationToken) 
        { 
          var item = await _todoItemContext.TodoItems.FirstOrDefaultAsync(i => i.Id == todoItemId);
          item.Title = changeTitleCommand.Title;
          await _todoItemContext.SaveChangesAsync();
          return NoContent();
        }

        [HttpGet("{todoItemId}")]
        public async Task<TodoItem> GetItemById([FromRoute] Guid todoItemId)
        {
            var item = await _todoItemContext.TodoItems.FirstOrDefaultAsync(i => i.Id == todoItemId);
            return mapEntity(item);
        }

        [HttpGet]
        public async Task<IEnumerable<TodoItem>> GetAllItems()
        {
            var items = await _todoItemContext.TodoItems.ToListAsync();
            return items.Select(i => mapEntity(i));
        }

        private Persistence.Entities.TodoItem mapCreateCommand(CreateTodoItemCommand createItemCommand)
        {
            var entity = new Persistence.Entities.TodoItem();

            entity.Id = Guid.NewGuid();
            entity.Title = createItemCommand.Title;
            entity.IsCompleted = false;

            return entity;
        }

        private static TodoItem mapEntity(Persistence.Entities.TodoItem entity)
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
