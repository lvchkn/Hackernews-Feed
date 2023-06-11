using Application.Users;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Utils;

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

    public async Task<User> GetByIdAsync(int id)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .Include(u => u.Interests)
            .Include(u => u.FavouriteStories)
            .AsSplitQuery()
            .Where(u => u.Id == id)
            .SingleOrDefaultAsync();

        if (user is null)
        {
            throw new NotFoundException("No user found with this id.");
        }

        return user;
    }

    public async Task AddAsync(User newUser)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .Where(u => u.Id == newUser.Id)
            .SingleOrDefaultAsync();

        if (user is not null)
        {
            throw new AlreadyExistsException("Id is already in use.");
        }

        await _dbContext.Users.AddAsync(newUser);

        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateLastActiveAsync(int id)
    {
        var user = await GetByIdAsync(id);
        
        if (user is null)
        {
            throw new NotFoundException("No user found with this id.");
        }

        user = user with { LastActive = DateTime.UtcNow };
        _dbContext.Update(user);

        await _dbContext.SaveChangesAsync();
    }

    public async Task AddInterestAsync(int id, int interestId)
    {
        var user = await GetByIdAsync(id);
        
        if (user is null)
        {
            throw new NotFoundException("No user found with this email.");
        }

        var interest = await _dbContext.Interests
            .Where(i => i.Id == interestId)
            .SingleOrDefaultAsync();
        
        if (interest is null)
        {
            throw new NotFoundException("No interest found with this id.");
        }

        user.Interests.Add(interest);
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteInterestAsync(int id, int interestId)
    {
        var user = await GetByIdAsync(id);
        
        if (user is null)
        {
            throw new NotFoundException("No user found with this id.");
        }

        var interest = await _dbContext.Interests
            .Where(i => i.Id == interestId)
            .SingleOrDefaultAsync();
        
        if (interest is null)
        {
            throw new NotFoundException("No interest found with this id.");
        }

        user.Interests.Remove(interest);
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Interest>> GetInterestsAsync(int id)
    {
        var user = await GetByIdAsync(id);
        
        if (user is null)
        {
            throw new NotFoundException("No user found with this id.");
        }   
        
        return user.Interests.ToList();
    }
}