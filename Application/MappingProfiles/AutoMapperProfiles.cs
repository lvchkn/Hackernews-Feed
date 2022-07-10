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
        }
    }
}
