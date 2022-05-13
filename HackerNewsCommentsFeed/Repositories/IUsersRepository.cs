using HackerNewsCommentsFeed.Domain;

namespace HackerNewsCommentsFeed.Repositories;

public interface IUsersRepository
{
    Task UpdateLastActiveAsync(string? email);
    Task<IEnumerable<User>> GetUsersAsync();
    Task AddUserAsync(User user);
}