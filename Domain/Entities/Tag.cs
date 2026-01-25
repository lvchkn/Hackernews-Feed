namespace Domain.Entities;

public class Tag
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public List<TagAlias> Aliases { get; init; } = [];
    public List<Story> Stories { get; init; } = [];
}