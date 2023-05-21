namespace Application.Stories;

public interface IStoriesService
{
    Task AddAsync(StoryDto storyDto);
    Task<List<StoryDto>> GetAllAsync();
    List<StoryDto> GetStories(string? orderBy, string? search);
}