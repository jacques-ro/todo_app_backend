
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Backend.Commands;
using Todo.Backend.DTOs;
using Todo.Backend.Persistence.Context;
using Todo.Backend.Queries;

namespace Todo.Backend.Controllers
{

    [ApiVersion("1.0")]
    [Route("/api/v{version:apiVersion}/todos")]
    [ApiController]
    [Authorize]
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

        [HttpPost("{todoItemId}/changetitle")]
        public async Task<IActionResult> ChangeTodoItemTitle(
            [FromRoute] Guid todoItemId,
            [FromBody] ChangeTodoItemTitleDTO changeTitleDTO,
            CancellationToken cancellationToken
        )
        {
          await _mediator.Send(new ChangeTodoItemTitleCommand { Id = todoItemId, Title = changeTitleDTO.Title }, cancellationToken);
          return NoContent();
        }

        [HttpPost("{todoItemId}/tickoff")]
        public async Task<IActionResult> TickOffTodo(
            [FromRoute] Guid todoItemId,
            CancellationToken cancellationToken
        )
        {
            await _mediator.Send(
                new TickOffTodoItemCommand() { Id = todoItemId },
                cancellationToken
            );

            return NoContent();
        }

        [HttpGet("{todoItemId}")]
        public async Task<IActionResult> GetItemById(
            [FromRoute] Guid todoItemId,
            CancellationToken cancellationToken
        )
        {
            var item = await _mediator.Send(
                new GetTodoItemByIdQuery { Id = todoItemId },
                cancellationToken
            );

            if(item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllItems(
            CancellationToken cancellationToken
        )
        {
            var items = await _mediator.Send(
                new GetTodoItemsQuery(),
                cancellationToken
            );

            return Ok(items);
        }

        [HttpPost("{todoItemId}/delete")]
        public async Task<IActionResult> DeleteItemById(
            [FromRoute] Guid todoItemId,
            CancellationToken cancellationToken
        )
        {
            await _mediator.Send(
                new DeleteTodoItemCommand() { Id = todoItemId },
                cancellationToken
            );

            return NoContent();
        }
    }
}
