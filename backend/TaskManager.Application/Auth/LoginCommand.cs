using MediatR;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Auth;

public class LoginCommand : IRequest<UserDto>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, UserDto>
{
    private readonly IUserRepository _users;

    public LoginCommandHandler(IUserRepository users)
    {
        _users = users;
    }

    public async Task<UserDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _users.GetByUsernameAsync(request.Username, cancellationToken)
            ?? throw new UnauthorizedAccessException("Invalid username or password.");

        if (user.PasswordHash != request.Password)
            throw new UnauthorizedAccessException("Invalid username or password.");

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username
        };
    }
}