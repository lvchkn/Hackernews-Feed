using Domain.Entities;

namespace Application.ApiConnections
{
    public interface IApiConnector
    {
        Task<Comment?> GetComment(int id);
        Task<Comment?> GetLastComment();
    }
}