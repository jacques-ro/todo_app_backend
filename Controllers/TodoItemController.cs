
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Todo.Backend.Commands;
using Todo.Backend.DTOs;
using Todo.Backend.Models;
using Todo.Backend.Persistence.Context;
using Todo.Backend.Queries;

namespace Todo.Backend.Controllers
{

    [Route("/api/todos/")]
    [ApiController]
    public class TodoItemController : ControllerBase
    {
        private readonly IMediator _mediator;      
        private readonly TodoItemContext _todoItemContext;  

        public TodoItemController(IMediator mediator, TodoItemContext todoItemContext)
        {
            _mediator = mediator;
            _todoItemContext = todoItemContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodoItem(
            [FromBody] CreateTodoItemDTO createItemDTO,
            CancellationToken cancellationToken
        )
        {            
            var itemId = await _mediator.Send(new CreateTodoItemCommand { Title = createItemDTO.Title });

            return CreatedAtAction(nameof(GetItemById), new { todoItemId = itemId }, itemId);
        }
    
        [HttpPut("{todoItemId}")]
        public async Task<IActionResult> ChangeTodoItemTitle(
            [FromRoute] Guid todoItemId,
            [FromBody] ChangeTodoItemTitleDTO changeTitleDTO,
            CancellationToken cancellationToken
        ) 
        {
          await _mediator.Send(new ChangeTodoItemTitleCommand { Id = todoItemId, Title = changeTitleDTO.Title });
          return NoContent();
        }

        [HttpGet("{todoItemId}")]
        public async Task<TodoItem> GetItemById([FromRoute] Guid todoItemId)
        {
            return await _mediator.Send(
                new GetTodoItemByIdQuery { Id = todoItemId }
            );
        }

        [HttpGet]
        public async Task<IEnumerable<TodoItem>> GetAllItems()
        {
            return await _mediator.Send(new GetTodoItemsQuery());
        }      
    }
}
