namespace Domain.Entities;

public class TagAlias
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int TagId { get; init; }
}