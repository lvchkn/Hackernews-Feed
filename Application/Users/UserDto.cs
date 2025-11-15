using Application.Interests;
using Application.Stories;

namespace Application.Users;

public record UserDto
{
    public int? Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public DateTime LastActive { get; init; }
    public List<InterestDto> Interests { get; init; } = [];
    public List<StoryDto> FavouriteStories { get; init; } = [];
}