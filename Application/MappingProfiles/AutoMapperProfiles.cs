using Application.Interests;
using Application.Stories;
using Application.Tags;
using Application.Users;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles;

internal class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<InterestDto, Interest>().ReverseMap();
        CreateMap<UserDto, User>().ReverseMap();
        CreateMap<StoryDto, Story>().ReverseMap();
        CreateMap<TagDto, Tag>().ReverseMap();
    }
}