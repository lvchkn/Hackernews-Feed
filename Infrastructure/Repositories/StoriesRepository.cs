using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Utils;

namespace Infrastructure.Mongo.Repositories;

public class StoriesRepository : IStoriesRepository
{
    private readonly AppDbContext _dbContext;

    public StoriesRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Story?> GetByIdAsync(int id)
    {
        var story = await _dbContext.Stories
            .Where(i => i.Id == id)
            .SingleOrDefaultAsync();

        return story;
    }

    public async Task<List<Story>> GetByAuthorAsync(string author)
    {
        var stories = await _dbContext.Stories
            .Where(i => i.By == author)
            .ToListAsync();

        return stories;
    }

    public async Task<List<Story>> GetAllAsync()
    {
        var stories = await _dbContext.Stories.ToListAsync();

        return stories;
    }

    public async Task AddAsync(Story newStory)
    {
        var story = await GetByIdAsync(newStory.Id);

        if (story is not null) 
        {
            throw new AlreadyExistsException("This story already exists!");
        }

        await _dbContext.Stories.AddAsync(newStory);

        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, Story updatedStory)
    {
        var story = await GetByIdAsync(id);

        if (story is null) 
        {
            throw new NotFoundException("This story doesn't exist!");
        }
        
        updatedStory = updatedStory with { Id = id };
        _dbContext.Update(updatedStory);

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var story = await GetByIdAsync(id);

        if (story is null) 
        {
            throw new NotFoundException("This story doesn't exist!");
        }

        _dbContext.Stories.Remove(story);
        
        await _dbContext.SaveChangesAsync();
    }
}