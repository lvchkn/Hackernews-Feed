namespace Application.Contracts;

public record TagDto
{
    public int? Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public List<StoryDto> Stories { get; init; } = new ();
}