using Domain.Entities;

namespace Application.Interests;

public interface IInterestsRepository
{
    Task<Interest?> GetByNameAsync(string name);
    Task<Interest?> GetByIdAsync(int id);
    Task<List<Interest>> GetAllAsync();
    Task<int?> AddAsync(Interest interest);
    Task<bool> UpdateAsync(int id, Interest updatedInterest);
    Task<bool> DeleteAsync(int id);
}