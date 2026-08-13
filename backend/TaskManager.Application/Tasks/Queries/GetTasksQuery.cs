using MediatR;
using TaskManager.Application.Common;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Tasks.Queries;

public class GetTasksQuery : IRequest<PagedResult<TaskDto>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
    public bool? IsCompleted { get; set; }
    public string? SortBy { get; set; }
    public string? SortDir { get; set; }
}

public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, PagedResult<TaskDto>>
{
    private readonly ITaskRepository _tasks;

    public GetTasksQueryHandler(ITaskRepository tasks)
    {
        _tasks = tasks;
    }

    public async Task<PagedResult<TaskDto>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _tasks.GetPagedAsync(
            request.Page,
            request.PageSize,
            request.Search,
            request.IsCompleted,
            request.SortBy,
            request.SortDir,
            cancellationToken);

        return new PagedResult<TaskDto>
        {
            Items = items.Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                IsCompleted = t.IsCompleted,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            }).ToList(),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}