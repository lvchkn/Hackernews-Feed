using Application.Interfaces;

namespace Application.Contracts;

public record CommentDto : IMessage
{
    public int Id { get; init; }
    public string By { get; init; } = string.Empty;
    public int[] Kids { get; init; } = Array.Empty<int>();
    public int Parent { get; init; }
    public string Text { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public int Time { get; init; }
}