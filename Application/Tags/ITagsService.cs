using Application.Paging;
using Domain.Entities;

namespace Application.Tags;

public interface ITagsService
{
    Task<List<TagDto>> GetAllAsync();
    Task<PagedTagsDto> GetPagedAsync(int pageNumber, int pageSize);
    Task<List<Tag>> GetTagsForTextAsync(string text);
    Task SeedAsync();
}