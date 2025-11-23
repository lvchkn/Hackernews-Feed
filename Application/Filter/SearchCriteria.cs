namespace Application.Filter;

public record SearchCriteria
{
    public string? Title { get; } = string.Empty;
    public int MinScore { get; init; }

    public SearchCriteria(string? title, int minScore)
    {
        Title = title;
        MinScore = minScore;
    }
}