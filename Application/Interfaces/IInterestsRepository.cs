using Domain.Entities;

namespace Application.Interfaces;
public interface IInterestsRepository
{
    Task<Interest?> GetByNameAsync(string name);
    Task<Interest?> GetByIdAsync(int id);
    Task<List<Interest>> GetAllAsync();
    Task<int> AddAsync(Interest interest);
    Task UpdateAsync(int id, Interest updatedInterest);
    Task DeleteAsync(int id);
}