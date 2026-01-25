using Application.Paging;
using Application.Tags;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Db.Repositories;

public class TagsRepository : ITagsRepository
{
    private readonly AppDbContext _dbContext;

    public TagsRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedTags> GetPagedAsync(int skip, int take)
    {
        var tags = _dbContext.Tags
            .AsNoTracking()
            .Include(t => t.Aliases)
            .OrderBy(t => t.Name);

        var pagedTags = await PageAsync(tags, skip, take);

        return pagedTags;
    }

    public async Task<List<Tag>> GetAllAsync()
    {
        var tags = await _dbContext.Tags
            .AsNoTracking()
            .Include(t => t.Aliases)
            .ToListAsync();

        return tags;
    }

    public async Task<bool> ExistsAsync()
    {
        return await _dbContext.Tags.AnyAsync();
    }

    public async Task AddRangeAsync(IEnumerable<Tag> tags)
    {
        await _dbContext.Tags.AddRangeAsync(tags);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
        int changes = await _dbContext.SaveChangesAsync();
        return changes;
    }

    private async Task<PagedTags> PageAsync(IQueryable<Tag> tags, int skip, int take)
    {
        double filteredCount = await tags.CountAsync();
        var totalPagesCount = (int)Math.Ceiling(filteredCount / take);

        var pagedTags = await tags.Skip(skip)
            .Take(take)
            .ToListAsync();

        return new PagedTags(Tags: pagedTags, TotalPagesCount: totalPagesCount);
    }
}
