namespace Domain.Entities;

public record Comment
{
    public string Id { get; init; } = string.Empty;
    public string By { get; init; } = string.Empty;
    public int[] Kids { get; init; } = Array.Empty<int>();
    public int Parent { get; init; }
    public string Text { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public int Time { get; init; }
}