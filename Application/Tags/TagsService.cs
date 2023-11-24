using Application.Stories;

namespace Application.Tags;

public class TagsService : ITagsService
{
    private readonly TagsCache _tagsCache;
    
    public TagsService(TagsCache tagsCache)
    {
        _tagsCache = tagsCache;
    }
    
    public List<TagDto> GetAll(StoryDto story)
    {
        var cache = _tagsCache.Tags;
        var tags = new List<TagDto>();
        
        foreach (var tag in cache)
        {
            if (story.Title.Contains($" {tag} "))
            {
                tags.Add(new TagDto()
                {
                    Name = tag
                });
            }
        }

        return tags;
    }
}