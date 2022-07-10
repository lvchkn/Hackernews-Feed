using Application.Contracts;

namespace Application.Services.Comments
{
    public interface ICommentsService
    {
        Task<List<CommentDto>> GetAllAsync();
        Task AddAsync(CommentDto comment);
    }
}
