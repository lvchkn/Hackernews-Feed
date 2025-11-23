using Domain.Entities;

namespace Application.Users;

public interface IUsersRepository
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<bool> UpdateLastActiveAsync(int id);
    Task<bool> AddInterestAsync(int id, int interestId);
    Task AddAsync(User user);
    Task<bool> DeleteInterestAsync(int id, int interestId);
    Task<List<Interest>?> GetInterestsAsync(int id);
}