using Application.Services;
using Domain.Entities;

namespace Application.Interfaces;

public interface IStoriesRepository
{
    Task<Story?> GetByIdAsync(int id);
    Task<List<Story>> GetByAuthorAsync(string author);
    Task<List<Story>> GetAllAsync();
    List<Story> GetSortedStories(IEnumerable<SortingParameters> sortingParameters);
    Task AddAsync(Story story);
    Task UpdateAsync(int id, Story updatedStory);
    Task DeleteAsync(int id);
}