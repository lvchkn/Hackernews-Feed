using Application.Paging;

namespace Application.Tags;

public interface ITagsService
{
    Task<List<TagDto>> GetAllAsync();
    Task<PagedTagsDto> GetPagedAsync(int pageNumber, int pageSize);
    Task SeedAsync();
}