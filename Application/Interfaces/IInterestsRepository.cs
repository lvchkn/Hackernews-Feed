using Domain.Entities;

namespace Application.Interfaces;
public interface IInterestsRepository
{
    Task<Interest> GetByNameAsync(string name);
    Task<List<Interest>> GetAllAsync();
    Task<string> AddAsync(Interest interest);
    Task<string> UpdateAsync(string id, Interest updatedInterest);
    Task<string> DeleteAsync(string id);
}