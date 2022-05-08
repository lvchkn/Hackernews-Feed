using HackerNewsCommentsFeed.Domain;

namespace HackerNewsCommentsFeed.ApiConnections;

public interface IApiConnector
{
    Task<Comment?> GetComment(int id);
    Task<Comment?> GetLastComment();
}