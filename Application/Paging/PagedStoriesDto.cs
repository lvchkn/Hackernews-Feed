using Application.Stories;

namespace Application.Paging;

public record PagedStoriesDto
{
    public List<StoryDto> Stories { get; init; } = new ();
    public int TotalPagesCount { get; init; }
}