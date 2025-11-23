using Domain.Entities;

namespace Application.Paging;

public record PagedStories
{
    public List<Story> Stories { get; init; } = [];
    public int TotalPagesCount { get; init; }
}