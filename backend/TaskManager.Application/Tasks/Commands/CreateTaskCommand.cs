using MediatR;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
namespace TaskManager.Application.Tasks.Commands;
public class CreateTaskCommand : IRequest<TaskDto>
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
}
public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
{
    private readonly ITaskRepository _tasks;
    private readonly IUnitOfWork _unitOfWork;
    public CreateTaskCommandHandler(ITaskRepository tasks, IUnitOfWork unitOfWork)
    {
        _tasks = tasks;
        _unitOfWork = unitOfWork;
    }
    public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var existing = await _tasks.GetByTitleAsync(request.Title.Trim(), cancellationToken);
        if (existing is not null)
            throw new InvalidOperationException("A task with this name already exists.");

        var now = DateTime.UtcNow;
        var task = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            IsCompleted = false,
            CreatedAt = now,
            UpdatedAt = now
        };
        await _tasks.AddAsync(task, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
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