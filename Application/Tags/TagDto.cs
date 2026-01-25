namespace Application.Tags;

public record TagDto
{
    public string Name { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public List<TagAliasDto> Aliases { get; init; } = [];
}