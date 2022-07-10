using Application.Contracts;

namespace Application.ApiConnections
{
    public interface IApiConnector
    {
        Task<CommentDto?> GetComment(int id);
        Task<CommentDto?> GetLastComment();
    }
}