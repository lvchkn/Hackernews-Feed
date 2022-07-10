using HackerNewsCommentsFeed.Domain;

namespace HackerNewsCommentsFeed.Repositories;

public interface ICommentsRepository
{
    Task<List<Comment>> GetAllAsync();
    Task AddAsync(Comment comment);
}