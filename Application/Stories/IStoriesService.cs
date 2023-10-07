using Application.Paging;

namespace Application.Stories;

public interface IStoriesService
{
    Task AddAsync(StoryDto storyDto);
    Task<List<StoryDto>> GetAllAsync();
    PagedStoriesDto GetStories(string? orderBy, string? search, int pageNumber, int pageSize);
}