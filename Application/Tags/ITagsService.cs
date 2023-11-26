using Application.Stories;

namespace Application.Tags;

public interface ITagsService
{
    List<TagDto> GetAll(StoryDto story);
}