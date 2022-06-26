using HackerNewsCommentsFeed.Domain;

namespace HackerNewsCommentsFeed.Repositories;

public interface IUsersRepository
{
    Task<List<User>> GetAllAsync();
    Task<User> GetByEmail(string email);
    Task<string> UpdateLastActiveAsync(string email);
    Task<string> UpdateInterestsAsync(string email, IEnumerable<string> interestIds);
    Task<string> AddInterestAsync(string email, string interestId);
    Task<string> AddAsync(User user);
    Task<string> DeleteInterestAsync(string email, string interestId);
}