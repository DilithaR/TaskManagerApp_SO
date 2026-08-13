using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Tasks.Commands;
using TaskManager.Application.Tasks.Queries;

namespace TaskManager.API.Controllers;

[Authorize]
[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] bool? isCompleted = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortDir = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetTasksQuery
        {
            Page = page,
            PageSize = pageSize,
            Search = search,
            IsCompleted = isCompleted,
            SortBy = sortBy,
            SortDir = sortDir
        }, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetTaskByIdQuery { Id = id }, cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskCommand command, CancellationToken cancellationToken)
    {
        var created = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateTaskCommand command, CancellationToken cancellationToken)
    {
        command.Id = id;
        return Ok(await _mediator.Send(command, cancellationToken));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteTaskCommand { Id = id }, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:int}/complete")]
    public async Task<IActionResult> ToggleComplete(int id, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new ToggleCompleteCommand { Id = id }, cancellationToken));
    }
}