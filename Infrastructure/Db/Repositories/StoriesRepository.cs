using Application.Stories;
using Application.Filter;
using Application.Paging;
using Application.Ranking;
using Application.Sort;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Db.Repositories;

public class StoriesRepository : IStoriesRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IQuerySort<Story> _querySort;
    private readonly IQueryFilter<Story> _queryFilter;
    private readonly IQueryRank<Story> _queryRank;

    public StoriesRepository(AppDbContext dbContext, 
        IQuerySort<Story> storiesQuerySort, 
        IQueryFilter<Story> queryFilter, 
        IQueryRank<Story> queryRank)
    {
        _dbContext = dbContext;
        _querySort = storiesQuerySort;
        _queryFilter = queryFilter;
        _queryRank = queryRank;
    }

    public async Task<Story?> GetByIdAsync(int id)
    {
        var story = await _dbContext.Stories
            .AsNoTracking()
            .Include(s => s.Tags)
            .Include(s => s.FavouritedBy)
            .AsSplitQuery()
            .SingleOrDefaultAsync(i => i.Id == id);

        return story;
    }
    
    private async Task<Story?> GetByIdAsyncWithTracking(int id)
    {
        var story = await _dbContext.Stories
            .Include(s => s.Tags)
            .Include(s => s.FavouritedBy)
            .AsSplitQuery()
            .SingleOrDefaultAsync(i => i.Id == id);

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
    
    public async Task<PagedStories> GetPagedAsync(IEnumerable<SortParameters> sortingParameters, 
        SearchCriteria searchCriteria, 
        int skip, 
        int take)
    {
        var filteredStories = Filter(searchCriteria);
        var sortedStories = Sort(filteredStories, sortingParameters);
        var rankedStories = Rank(sortedStories);
        var pagedStories = await PageAsync(rankedStories, skip, take);

        return pagedStories;
    }

    public async Task<bool> AddAsync(Story newStory)
    {
        var story = await GetByIdAsyncWithTracking(newStory.Id);

        if (story is not null)
        {
            return false;
        }

        newStory.Rank = _queryRank.CalculateRank(newStory);
        await _dbContext.Stories.AddAsync(newStory);
        
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateAsync(int id, Story updatedStory)
    {
        var story = await GetByIdAsyncWithTracking(id);

        if (story is null)
        {
            return false;
            //throw new NotFoundException("This story doesn't exist!");
        }

        updatedStory.Id = id;
        _dbContext.Update(updatedStory);
        
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var story = await GetByIdAsyncWithTracking(id);

        if (story is null) 
        {
            return false;
            //throw new NotFoundException("This story doesn't exist!");
        }

        _dbContext.Stories.Remove(story);
        await _dbContext.SaveChangesAsync();
        
        return true;
    }
    
    private IQueryable<Story> Filter(SearchCriteria searchCriteria)
    {
        var stories = _dbContext.Stories
            .TagWith("Sort and paginate")
            .AsNoTracking()
            .Include(s => s.Tags)
            .Include(s => s.FavouritedBy)
            .AsSplitQuery();

        var filteredStories = _queryFilter.Filter(stories, searchCriteria);
        
        return filteredStories;
    }
    
    private IQueryable<Story> Sort(IQueryable<Story> stories, IEnumerable<SortParameters> sortingParameters)
    {
        var sortedStories = _querySort.Sort(stories, sortingParameters);
        return sortedStories;
    }
    
    private IQueryable<Story> Rank(IQueryable<Story> stories)
    {
        var rankedStories = stories.OrderBy(s => s.Rank);
        return rankedStories;
    }

    private async Task<PagedStories> PageAsync(IQueryable<Story> stories, int skip, int take)
    {
        double filteredStoriesCount = await stories.CountAsync();
        var totalPagesCount = (int)Math.Ceiling(filteredStoriesCount / take);

        var paginatedStories = await stories.Skip(skip)
            .Take(take)
            .ToListAsync();

        return new PagedStories
        {
            Stories = paginatedStories,
            TotalPagesCount = totalPagesCount,
        };
    }
}