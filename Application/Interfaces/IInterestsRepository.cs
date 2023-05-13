using Domain.Entities;

namespace Application.Interfaces;
public interface IInterestsRepository
{
    Task<Interest?> GetByNameAsync(string name);
    Task<List<Interest>> GetAllAsync();
    Task AddAsync(Interest interest);
    Task UpdateAsync(int id, Interest updatedInterest);
    Task DeleteAsync(int id);
}