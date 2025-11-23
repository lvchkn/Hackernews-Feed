using Application.Users;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Db.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly AppDbContext _dbContext;

    public UsersRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<User>> GetAllAsync()
    {
        var users = await _dbContext.Users
            .AsNoTracking()
            .Include(u => u.Interests)
            .Include(u => u.FavouriteStories)
            .AsSplitQuery()
            .ToListAsync();

        return users;
    }
    
    public async Task<User?> GetByIdAsync(int id)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .Include(u => u.Interests)
            .Include(u => u.FavouriteStories)
            .AsSplitQuery()
            .SingleOrDefaultAsync(u => u.Id == id);

        return user;
    }
    
    private async Task<User?> GetByIdWithTrackingAsync(int id)
    {
        var user = await _dbContext.Users
            .Include(u => u.Interests)
            .Include(u => u.FavouriteStories)
            .AsSplitQuery()
            .SingleOrDefaultAsync(u => u.Id == id);

        return user;
    }

    public async Task AddAsync(User newUser)
    {
        await _dbContext.Users.AddAsync(newUser);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> UpdateLastActiveAsync(int id)
    {
        var user = await GetByIdWithTrackingAsync(id);
        
        if (user is null)
        {
            return false;
        }

        user.LastActive = DateTime.UtcNow;
        _dbContext.Update(user);

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> AddInterestAsync(int id, int interestId)
    {
        var user = await GetByIdWithTrackingAsync(id);
        
        if (user is null)
        {
            return false;
        }

        var interest = await _dbContext.Interests
            .SingleOrDefaultAsync(i => i.Id == interestId);
        
        if (interest is null)
        {
            return false;
        }

        user.Interests.Add(interest);
        
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteInterestAsync(int id, int interestId)
    {
        var user = await GetByIdWithTrackingAsync(id);
        
        if (user is null)
        {
            return false;
        }

        var interest = await _dbContext.Interests.FindAsync(interestId);
        
        if (interest is null)
        {
            return false;
        }

        user.Interests.Remove(interest);
        
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<List<Interest>?> GetInterestsAsync(int id)
    {
        var user = await GetByIdAsync(id);
        
        if (user is null)
        {
            return null;
        }   
        
        return user.Interests.ToList();
    }
}