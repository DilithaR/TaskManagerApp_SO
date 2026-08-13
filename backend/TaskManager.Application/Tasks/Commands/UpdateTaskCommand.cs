using MediatR;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Interfaces;
namespace TaskManager.Application.Tasks.Commands;
public class UpdateTaskCommand : IRequest<TaskDto>
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
}
public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TaskDto>
{
    private readonly ITaskRepository _tasks;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateTaskCommandHandler(ITaskRepository tasks, IUnitOfWork unitOfWork)
    {
        _tasks = tasks;
        _unitOfWork = unitOfWork;
    }
    public async Task<TaskDto> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _tasks.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Task {request.Id} was not found.");
        var existing = await _tasks.GetByTitleAsync(request.Title.Trim(), cancellationToken);
            if (existing is not null && existing.Id != request.Id)
                throw new InvalidOperationException("A task with this name already exists.");
        task.Title = request.Title;
        task.Description = request.Description;
        task.UpdatedAt = DateTime.UtcNow;
        await _tasks.UpdateAsync(task, cancellationToken);
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