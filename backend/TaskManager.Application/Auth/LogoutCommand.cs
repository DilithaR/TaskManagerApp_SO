using MediatR;

namespace TaskManager.Application.Auth;

public class LogoutCommand : IRequest
{
}

public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
{
    public Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}