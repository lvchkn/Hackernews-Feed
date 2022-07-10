namespace Domain.Entities;

public record Interest
{
    public string Id { get; init; } = string.Empty;

    public string Text { get; init; } = string.Empty;
}