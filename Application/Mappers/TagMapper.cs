using Application.Tags;
using Domain.Entities;

namespace Application.Mappers;

public static class TagMapper
{
    public static Tag ToTag(this TagDto tagDto)
    {
        return new Tag
        {
            Id = tagDto.Id,
            Name = tagDto.Name,
        };
    }
    
    public static TagDto ToTagDto(this Tag tag)
    {
        return new TagDto
        {
            Id = tag.Id,
            Name = tag.Name,
        };
    }
}