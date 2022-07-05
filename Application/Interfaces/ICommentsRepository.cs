using Domain.Entities;

namespace Application.Interfaces;

public interface ICommentsRepository
{
    Task<List<Comment>> GetAllAsync();
    Task AddAsync(Comment comment);
}