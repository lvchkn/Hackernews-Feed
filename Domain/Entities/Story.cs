using Domain.Processing;

namespace Domain.Entities;

public record Story : ISortable, IFilterable
{
    public string By { get; init; } = string.Empty;
    public int Descendants { get; init; }
    public int Id { get; init; }
    public int[] Kids { get; init; } = [];
    public int Score { get; init; }
    public int Time { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
    public string Text { get; init; } = string.Empty;
    public List<Tag> Tags { get; init; } = [];
    public List<User> FavouritedBy { get; init; } = [];
}