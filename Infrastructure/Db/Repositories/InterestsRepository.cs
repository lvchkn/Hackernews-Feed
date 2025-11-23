using Application.Interests;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Db.Repositories;

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
            .SingleOrDefaultAsync(i => i.Id == id);

        return interest;
    }
    
    private async Task<Interest?> GetByIdAsyncWithTracking(int id)
    {
        var interest = await _dbContext.Interests.FindAsync(id);

        return interest;
    }

    public async Task<Interest?> GetByNameAsync(string name)
    {
        var interest = await _dbContext.Interests
            .AsNoTracking()
            .SingleOrDefaultAsync(i => i.Text == name);

        return interest;
    }

    public async Task<List<Interest>> GetAllAsync()
    {
        var interests = await _dbContext.Interests
            .AsNoTracking()
            .ToListAsync();

        return interests;
    }

    public async Task<int?> AddAsync(Interest newInterest)
    {
        var interest = await _dbContext.Interests
            .SingleOrDefaultAsync(i => i.Text == newInterest.Text);

        if (interest is not null) 
        {
            return null;
        }

        var result = await _dbContext.Interests.AddAsync(newInterest);
        
        await _dbContext.SaveChangesAsync();

        return result.Entity.Id;
    }

    public async Task<bool> UpdateAsync(int id, Interest updatedInterest)
    {
        var interest = await GetByIdAsync(id);

        if (interest is null) 
        {
            return false;
        }

        updatedInterest.Id = id;
        _dbContext.Update(updatedInterest);

        await _dbContext.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var interest = await GetByIdAsyncWithTracking(id);

        if (interest is null) 
        {
            return false;
        }

        _dbContext.Interests.Remove(interest);
        
        await _dbContext.SaveChangesAsync();
        
        return true;
    }
}