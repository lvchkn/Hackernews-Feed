using Domain.Entities;

namespace Application.Paging;

public record PagedTags(List<Tag> Tags, int TotalPagesCount);