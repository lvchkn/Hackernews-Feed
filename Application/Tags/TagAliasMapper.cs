using Domain.Entities;

namespace Application.Tags;

public static class TagAliasMapper
{
    public static TagAlias ToTag(this TagAliasDto tagAliasDto)
    {
        return new TagAlias
        {
            Name = tagAliasDto.Name,
        };
    }

    public static TagAliasDto ToTagAliasDto(this TagAlias tagAlias)
    {
        return new TagAliasDto
        {
            Name = tagAlias.Name,
        };
    }
}