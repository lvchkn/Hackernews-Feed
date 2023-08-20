using Application.Filter;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Db;

public class Filterer : IFilterer
{
    public IQueryable<T> Filter<T>(IQueryable<T> entities, string? search) where T : IFilterable
    {
        var filteredStories = entities.Where(entity => EF.Functions.ILike(entity.Title, $"%{search}%"));

        return filteredStories;
    }
}