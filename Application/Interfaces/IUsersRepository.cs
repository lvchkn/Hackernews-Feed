using Domain.Entities;

namespace Application.Interfaces;
public interface IUsersRepository
{
    Task<List<User>> GetAllAsync();
    Task<User> GetByEmailAsync(string email);
    Task<string> UpdateLastActiveAsync(string email);
    Task<string> UpdateInterestsAsync(string email, IEnumerable<string> interestIds);
    Task<string> AddInterestAsync(string email, Interest interest);
    Task<string> AddAsync(User user);
    Task<string> DeleteInterestAsync(string email, string interestId);
    Task<List<string>> GetInterestsNamesAsync(string email);
    Task<List<string>> GetInterestsAsync(string email);
}