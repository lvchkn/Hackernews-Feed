using Application.Interfaces;

namespace Application.Contracts;

public record StoryDto : IMessage
{
    public string By { get; init; } = string.Empty;
    public int Descendants { get; init; }
    public int Id { get; init; }
    public int[] Kids { get; init; } = Array.Empty<int>();
    public int Score { get; init; }
    public int Time { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
}