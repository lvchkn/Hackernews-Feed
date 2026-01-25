using Application.Tags;

namespace Application.Paging;

public record PagedTagsDto
{
    public List<TagDto> Tags { get; init; } = [];
    public int TotalPagesCount { get; init; }
}
