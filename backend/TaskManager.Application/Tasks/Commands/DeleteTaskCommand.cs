using MediatR;
using TaskManager.Domain.Interfaces;
namespace TaskManager.Application.Tasks.Commands;
public class DeleteTaskCommand : IRequest
{
    public int Id { get; set; }
}
public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand>
{
    private readonly ITaskRepository _tasks;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteTaskCommandHandler(ITaskRepository tasks, IUnitOfWork unitOfWork)
    {
        _tasks = tasks;
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _tasks.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Task {request.Id} was not found.");
        await _tasks.DeleteAsync(task, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}