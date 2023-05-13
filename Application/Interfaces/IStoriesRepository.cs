using Domain.Entities;

namespace Application.Interfaces;

public interface IStoriesRepository
{
    Task<Story?> GetByIdAsync(int id);
    Task<List<Story>> GetByAuthorAsync(string author);
    Task<List<Story>> GetAllAsync();
    Task AddAsync(Story story);
    Task UpdateAsync(int id, Story updatedStory);
    Task DeleteAsync(int id);
}