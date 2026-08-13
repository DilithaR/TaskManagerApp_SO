using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _db;

    public TaskRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<(IReadOnlyList<TaskItem> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        string? search,
        bool? isCompleted,
        string? sortBy,
        string? sortDir,
        CancellationToken cancellationToken = default)
    {
        var query = _db.Tasks.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(t => t.Title.Contains(search) || (t.Description != null && t.Description.Contains(search)));

        if (isCompleted.HasValue)
            query = query.Where(t => t.IsCompleted == isCompleted.Value);

        var descending = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase);
        query = (sortBy?.ToLowerInvariant()) switch
        {
            "title" => descending ? query.OrderByDescending(t => t.Title) : query.OrderBy(t => t.Title),
            "iscompleted" => descending ? query.OrderByDescending(t => t.IsCompleted) : query.OrderBy(t => t.IsCompleted),
            "updatedat" => descending ? query.OrderByDescending(t => t.UpdatedAt) : query.OrderBy(t => t.UpdatedAt),
            _ => descending ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt)
        };

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public Task<TaskItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _db.Tasks.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task AddAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        await _db.Tasks.AddAsync(task, cancellationToken);
    }

    public Task UpdateAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        _db.Tasks.Update(task);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        _db.Tasks.Remove(task);
        return Task.CompletedTask;
    }
}