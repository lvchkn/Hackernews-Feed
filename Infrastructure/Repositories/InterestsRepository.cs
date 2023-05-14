using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Utils;

namespace Infrastructure.Repositories;

public class InterestsRepository : IInterestsRepository
{
    private readonly AppDbContext _dbContext;

    public InterestsRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Interest?> GetByIdAsync(int id)
    {
        var interest = await _dbContext.Interests
            .AsNoTracking()
            .Where(i => i.Id == id)
            .SingleOrDefaultAsync();

        return interest;
    }

    public async Task<Interest?> GetByNameAsync(string name)
    {
        var interest = await _dbContext.Interests
            .AsNoTracking()
            .Where(i => i.Text == name)
            .SingleOrDefaultAsync();

        return interest;
    }

    public async Task<List<Interest>> GetAllAsync()
    {
        var interests = await _dbContext.Interests
            .AsNoTracking()
            .ToListAsync();

        return interests;
    }

    public async Task<int> AddAsync(Interest newInterest)
    {
        var interest = await GetByNameAsync(newInterest.Text);

        if (interest is not null) 
        {
            throw new AlreadyExistsException("This interest already exists!");
        }

        await _dbContext.Interests.AddAsync(newInterest);
        
        await _dbContext.SaveChangesAsync();

        return newInterest.Id;
    }

    public async Task UpdateAsync(int id, Interest updatedInterest)
    {
        var interest = await GetByIdAsync(id);

        if (interest is null) 
        {
            throw new NotFoundException("This interest doesn't exist!");
        }
        
        updatedInterest = updatedInterest with { Id = id };
        _dbContext.Update(updatedInterest);

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var interest = await GetByIdAsync(id);

        if (interest is null) 
        {
            throw new NotFoundException("This interest doesn't exist!");
        }

        _dbContext.Interests.Remove(interest);
        
        await _dbContext.SaveChangesAsync();
    }
}