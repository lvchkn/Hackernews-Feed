using Application.Paging;
using Domain.Entities;

namespace Application.Tags;

public interface ITagsRepository
{
    Task<List<Tag>> GetAllAsync();
    Task<PagedTags> GetPagedAsync(int skip, int take);
    Task<bool> ExistsAsync();
    Task AddRangeAsync(IEnumerable<Tag> tags);
}
