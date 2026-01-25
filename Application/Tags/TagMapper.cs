using Domain.Entities;

namespace Application.Tags;

public static class TagMapper
{
    public static Tag ToTag(this TagDto tagDto)
    {
        return new Tag
        {
            Name = tagDto.Name,
            Category = tagDto.Category,
            Aliases = tagDto.Aliases.Select(a => a.ToTag()).ToList(),
        };
    }

    public static TagDto ToTagDto(this Tag tag)
    {
        return new TagDto
        {
            Name = tag.Name,
            Category = tag.Category,
            Aliases = tag.Aliases.Select(a => a.ToTagAliasDto()).ToList(),
        };
    }
}