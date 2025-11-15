namespace Domain.Entities;

public record Tag
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public List<Story> Stories { get; init; } = [];
}