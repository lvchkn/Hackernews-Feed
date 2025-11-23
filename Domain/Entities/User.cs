namespace Domain.Entities;

public class User
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public DateTime LastActive { get; set; }
    public List<Interest> Interests { get; init; } = [];
    public List<Story> FavouriteStories { get; init; } = [];
}