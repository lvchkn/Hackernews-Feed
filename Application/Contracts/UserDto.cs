namespace Application.Contracts;

public record UserDto
{
    public string? Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public DateTime LastActive { get; init; }
    public IEnumerable<string> InterestIds { get; init; } = new List<string>();
}