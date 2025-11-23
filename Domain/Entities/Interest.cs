namespace Domain.Entities;

public class Interest
{
    public int Id { get; set; }
    public string Text { get; init; } = string.Empty;
    public List<User> InterestedUsers { get; init; } = [];
}