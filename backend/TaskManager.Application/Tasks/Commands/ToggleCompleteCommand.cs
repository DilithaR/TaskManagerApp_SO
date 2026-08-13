using MediatR;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Interfaces;
namespace TaskManager.Application.Tasks.Commands;
public class ToggleCompleteCommand : IRequest<TaskDto>
{
    public int Id { get; set; }
}
public class ToggleCompleteCommandHandler : IRequestHandler<ToggleCompleteCommand, TaskDto>
{
    private readonly ITaskRepository _tasks;
    private readonly IUnitOfWork _unitOfWork;
    public ToggleCompleteCommandHandler(ITaskRepository tasks, IUnitOfWork unitOfWork)
    {
        _tasks = tasks;
        _unitOfWork = unitOfWork;
    }
    public async Task<TaskDto> Handle(ToggleCompleteCommand request, CancellationToken cancellationToken)
    {
        var task = await _tasks.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Task {request.Id} was not found.");
        task.IsCompleted = !task.IsCompleted;
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