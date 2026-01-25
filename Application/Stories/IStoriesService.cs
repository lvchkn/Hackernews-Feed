using Application.Paging;
using Application.Tags;

namespace Application.Stories;

public interface IStoriesService
{
    Task AddAsync(StoryDto storyDto);
    Task<List<StoryDto>> GetAllAsync();
    Task<List<TagDto>> GetTagsAsync(int storyId);
    Task<PagedStoriesDto> GetPagedAsync(string? orderBy, string? search, int pageNumber, int pageSize);
    Task<StoryDto> GetByIdAsync(int id);
}