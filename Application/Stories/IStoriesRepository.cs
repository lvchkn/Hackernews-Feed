using Application.Sort;
using Domain.Entities;

namespace Application.Stories;

public interface IStoriesRepository
{
    Task<Story?> GetByIdAsync(int id);
    Task<List<Story>> GetByAuthorAsync(string author);
    Task<List<Story>> GetAllAsync();
    (List<Story> paginatedStories, int totalPagesCount) GetAll(IEnumerable<SortingParameters> sortingParameters, 
        string? search, 
        int skip, 
        int take);
    Task AddAsync(Story story);
    Task UpdateAsync(int id, Story updatedStory);
    Task DeleteAsync(int id);
}