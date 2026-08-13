using MediatR;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Tasks.Queries;

public class GetTaskByIdQuery : IRequest<TaskDto>
{
    public int Id { get; set; }
}

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDto>
{
    private readonly ITaskRepository _tasks;

    public GetTaskByIdQueryHandler(ITaskRepository tasks)
    {
        _tasks = tasks;
    }

    public async Task<TaskDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await _tasks.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Task {request.Id} was not found.");

        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            IsCompleted = task.IsCompleted,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
    }
}
