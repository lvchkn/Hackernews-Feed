namespace Application.Interests;

public record InterestDto
{
    public int Id { get; init; }
    public string Text { get; init; } = string.Empty;
}