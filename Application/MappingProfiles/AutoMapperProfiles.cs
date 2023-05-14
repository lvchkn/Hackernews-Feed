using Application.Contracts;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
{
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
}
