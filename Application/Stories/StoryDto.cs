using Application.Messaging;
using Application.Tags;

namespace Application.Stories;

public record StoryDto : IMessage, IRankable
{
    public string By { get; init; } = string.Empty;
    public int Descendants { get; init; }
    public int Id { get; init; }
    public int[] Kids { get; init; } = Array.Empty<int>();
    public int Score { get; init; }
    public double Rank { get; set; }
    public int Time { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public List<TagDto> Tags { get; init; } = new ();
}