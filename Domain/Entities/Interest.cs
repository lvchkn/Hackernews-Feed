namespace Domain.Entities;

public record Interest
{
    public int Id { get; init; }
    public string Text { get; init; } = string.Empty;
    public List<User> InterestedUsers { get; init; } = new ();
}