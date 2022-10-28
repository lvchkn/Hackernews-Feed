using Application.Contracts;

namespace Application.Services.Stories;

public interface IStoriesService
{
    Task<string> AddAsync(StoryDto storyDto);
}