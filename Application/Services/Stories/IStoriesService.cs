using Application.Contracts;

namespace Application.Services.Stories;

public interface IStoriesService
{
    Task AddAsync(StoryDto storyDto);
}