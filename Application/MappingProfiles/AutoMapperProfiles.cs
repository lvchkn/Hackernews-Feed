using Application.Contracts;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
{
    internal class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CommentDto, Comment>().ReverseMap();
            CreateMap<InterestDto, Interest>().ReverseMap();
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<StoryDto, Story>();
                // .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                // .ReverseMap()
                // .ForMember(dest => dest.Id, opt => opt.MapFrom(src => int.Parse(src.Id)));
        }
    }
}
