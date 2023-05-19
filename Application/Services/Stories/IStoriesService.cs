using Application.Contracts;

namespace Application.Services.Stories;

public interface IStoriesService
{
    Task AddAsync(StoryDto storyDto);
    Task<List<StoryDto>> GetAllAsync();
    List<StoryDto> GetSortedStories(string? query);
}