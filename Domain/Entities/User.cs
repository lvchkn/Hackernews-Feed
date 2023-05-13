namespace Domain.Entities;

public record User
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public DateTime LastActive { get; init; }
    public List<Interest> Interests { get; init; } = new ();
    public List<Story> FavouriteStories { get; init; } = new ();
}