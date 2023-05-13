using Application.Interfaces;
using Domain.Entities;

namespace Infrastructure.Mongo.Repositories;

public class CommentsRepository : ICommentsRepository
{
    private readonly AppDbContext _dbContext;

    public CommentsRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Comment>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(Comment comment)
    {
        throw new NotImplementedException();
    }
}