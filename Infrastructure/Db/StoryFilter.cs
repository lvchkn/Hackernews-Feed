using Application.Filter;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Db;

public class StoryFilter : IQueryFilter<Story>
{
    public IQueryable<Story> Filter(IQueryable<Story> entities, SearchCriteria search)
    {
        if (string.IsNullOrWhiteSpace(search.Title))
        {
            return entities;
        }
        
        var filteredStories = entities.Where(entity => EF.Functions.ILike(entity.Title, $"%{search.Title}%"));

        return filteredStories;
    }
}