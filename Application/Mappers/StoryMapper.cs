using Application.Stories;
using Domain.Entities;

namespace Application.Mappers;

public static class StoryMapper
{
    public static Story ToStory(this StoryDto storyDto)
    {
        return new Story
        {
            By = storyDto.By,
            Descendants = storyDto.Descendants,
            Id = storyDto.Id,
            Kids = storyDto.Kids,
            Score = storyDto.Score,
            Time = storyDto.Time,
            Title = storyDto.Title,
            Url = storyDto.Url,
            Text = storyDto.Text,
            Tags = storyDto.Tags.Select(t => t.ToTag()).ToList(),
        };
    }

    public static StoryDto ToStoryDto(this Story story)
    {
        return new StoryDto
        {
            By = story.By,
            Descendants = story.Descendants,
            Id = story.Id,
            Kids = story.Kids,
            Score = story.Score,
            Time = story.Time,
            Title = story.Title,
            Url = story.Url,
            Text = story.Text,
            Tags = story.Tags.Select(t => t.ToTagDto()).ToList(),
            FavouritedBy = story.FavouritedBy.Select(u => u.Id).ToList(),
        };
    }
}