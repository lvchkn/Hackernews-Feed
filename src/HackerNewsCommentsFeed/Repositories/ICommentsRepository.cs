using HackerNewsCommentsFeed.Domain;

namespace HackerNewsCommentsFeed.Repositories;

public interface ICommentsRepository
{
    Task<IEnumerable<Comment>> GetCommentsAsync();
    Task AddCommentAsync(Comment comment);
}