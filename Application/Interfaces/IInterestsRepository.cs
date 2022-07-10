using Domain.Entities;

namespace Application.Interfaces;
public interface IInterestsRepository
{
    Task<Interest> GetByIdAsync(string id);
    Task<List<Interest>> GetAllAsync();
    Task<string> AddAsync(Interest interest);
    Task<string> UpdateAsync(string id, Interest updatedInterest);
    Task<string> DeleteAsync(string id);
}