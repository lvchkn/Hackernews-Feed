using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Utils;

namespace Infrastructure.Mongo.Repositories;

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
            .ToListAsync();

        return users;
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .Include(u => u.Interests)
            .Where(u => u.Email == email)
            .SingleOrDefaultAsync();

        if (user is null)
        {
            throw new NotFoundException("No user found with this email.");
        }

        return user;
    }

    public async Task AddAsync(User newUser)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .Where(u => u.Id == newUser.Id || u.Email == newUser.Email)
            .SingleOrDefaultAsync();

        if (user is not null)
        {
            throw new AlreadyExistsException("Email is already in use.");
        }

        await _dbContext.Users.AddAsync(newUser);

        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateLastActiveAsync(string email)
    {
        var user = await GetByEmailAsync(email);
        
        if (user is null)
        {
            throw new NotFoundException("No user found with this email.");
        }

        user = user with { LastActive = DateTime.UtcNow };
        _dbContext.Update(user);

        await _dbContext.SaveChangesAsync();
    }

    public async Task AddInterestAsync(string email, int interestId)
    {
        var user = await GetByEmailAsync(email);
        
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

    public async Task DeleteInterestAsync(string email, int interestId)
    {
        var user = await GetByEmailAsync(email);
        
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

        user.Interests.Remove(interest);
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Interest>> GetInterestsAsync(string email)
    {
        var user = await GetByEmailAsync(email);
        
        if (user is null)
        {
            throw new NotFoundException("No user found with this email.");
        }   
        
        return user.Interests.ToList();
    }
}