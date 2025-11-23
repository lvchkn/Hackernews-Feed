using Application.Stories;

namespace Application.Paging;

public record PagedStoriesDto
{
    public List<StoryDto> Stories { get; init; } = [];
    public int TotalPagesCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}