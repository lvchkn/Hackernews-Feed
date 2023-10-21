using Application.Stories;

namespace Application.Paging;

public static class PagingExtension
{
    public static PagedStoriesDto Paginate(this IEnumerable<StoryDto> stories, int skip, int take)
    {
        if (take <= 0) throw new ArgumentOutOfRangeException();

        var storyDtos = stories.ToList();
        
        var pagedStories = storyDtos.Skip(skip)
            .Take(take)
            .ToList();
        
        var totalPagesCount = (int)Math.Ceiling((double)storyDtos.Count / take);
        
        return new ()
        {
            Stories = pagedStories,
            TotalPagesCount = totalPagesCount,
        };
    }
}