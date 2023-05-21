using Application.Filter;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Db;

public class StoriesFilter : IFilter<Story>
{
    public IQueryable<Story> Filter(IQueryable<Story> stories, string? search)
    {
        var filteredStories = stories.Where(story => EF.Functions.ILike(story.Title, $"%{search}%"));

        return filteredStories;
    }
}