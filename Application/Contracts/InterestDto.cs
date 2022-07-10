namespace Application.Contracts;

public record InterestDto
{
    public string? Id { get; init; } = string.Empty;

    public string Text { get; init; } = string.Empty;
}