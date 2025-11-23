using Application.Filter;
using Application.Paging;
using Application.Sort;
using Domain.Entities;

namespace Application.Stories;

public interface IStoriesRepository
{
    Task<Story?> GetByIdAsync(int id);
    Task<List<Story>> GetByAuthorAsync(string author);
    Task<List<Story>> GetAllAsync();
    Task<PagedStories> GetPagedAsync(IEnumerable<SortParameters> sortingParameters, 
        SearchCriteria search, 
        int skip, 
        int take);
    Task<bool> AddAsync(Story story);
    Task<bool> UpdateAsync(int id, Story updatedStory);
    Task<bool> DeleteAsync(int id);
}