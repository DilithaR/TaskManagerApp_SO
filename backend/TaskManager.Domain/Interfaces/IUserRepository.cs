using TaskManager.Domain.Entities;
namespace TaskManager.Domain.Interfaces;
public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
}