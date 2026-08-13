using MediatR;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Auth;

public class GetCurrentUserQuery : IRequest<UserDto>
{
    public int UserId { get; set; }
}

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserDto>
{
    private readonly IUserRepository _users;

    public GetCurrentUserQueryHandler(IUserRepository users)
    {
        _users = users;
    }

    public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _users.GetByUsernameAsync(/* wait - we only have GetByUsername */)
    }
}