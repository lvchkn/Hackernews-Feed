namespace HackerNewsCommentsFeed.Domain;

public record Comment : IMessage
{
    public int Id { get; init; }
    public string By { get; init; }
    public int[] Kids { get; init; }
    public int Parent { get; init; }
    public string Text { get; init; }
    public string Type { get; init; }
    public int Time { get; init; }
}