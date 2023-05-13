using Domain.Entities;

namespace Application.Interfaces;
public interface IUsersRepository
{
    Task<List<User>> GetAllAsync();
    Task<User> GetByEmailAsync(string email);
    Task UpdateLastActiveAsync(string email);
    Task AddInterestAsync(string email, int interestId);
    Task AddAsync(User user);
    Task DeleteInterestAsync(string email, int interestId);
    Task<List<Interest>> GetInterestsAsync(string email);
}