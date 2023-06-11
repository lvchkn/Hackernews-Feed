using Domain.Entities;

namespace Application.Users;

public interface IUsersRepository
{
    Task<List<User>> GetAllAsync();
    Task<User> GetByIdAsync(int id);
    Task UpdateLastActiveAsync(int id);
    Task AddInterestAsync(int id, int interestId);
    Task AddAsync(User user);
    Task DeleteInterestAsync(int id, int interestId);
    Task<List<Interest>> GetInterestsAsync(int id);
}