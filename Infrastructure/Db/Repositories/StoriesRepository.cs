using Application.Stories;
using Application.Filter;
using Application.Sort;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Utils;

namespace Infrastructure.Db.Repositories;

public class StoriesRepository : IStoriesRepository
{
    private readonly AppDbContext _dbContext;
    private readonly ISorter _sorter;
    private readonly IFilterer _filterer;

    public StoriesRepository(
        AppDbContext dbContext, 
        ISorter storiesSorter, 
        IFilterer filter)
    {
        _dbContext = dbContext;
        _sorter = storiesSorter;
        _filterer = filter;
    }

    public async Task<Story?> GetByIdAsync(int id)
    {
        var story = await _dbContext.Stories
            .AsNoTracking()
            .Include(s => s.Tags)
            .Include(s => s.FavouritedBy)
            .AsSplitQuery()
            .Where(i => i.Id == id)
            .SingleOrDefaultAsync();

        return story;
    }

    public async Task<List<Story>> GetByAuthorAsync(string author)
    {
        var stories = await _dbContext.Stories
            .AsNoTracking()
            .Include(s => s.Tags)
            .Include(s => s.FavouritedBy)
            .AsSplitQuery()
            .Where(i => i.By == author)
            .ToListAsync();

        return stories;
    }

    public async Task<List<Story>> GetAllAsync()
    {
        var stories = await _dbContext.Stories
            .AsNoTracking()
            .Include(s => s.Tags)
            .Include(s => s.FavouritedBy)
            .AsSplitQuery()
            .ToListAsync();

        return stories;
    }

    public List<Story> GetAll(IEnumerable<SortingParameters> sortingParameters, string? search, int skip, int take)
    {
        var included = _dbContext.Stories
            .AsNoTracking()
            .Include(s => s.Tags)
            .Include(s => s.FavouritedBy)
            .AsSplitQuery();

        var filteredStories = _filterer.Filter(included, search);

        var sortedStories = _sorter
            .Sort(filteredStories, sortingParameters)
            .Skip(skip)
            .Take(take)
            .ToList();

        return sortedStories;
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