using Application.Users;
using Domain.Entities;

namespace Application.Mappers;

public static class UserMapper
{
    public static User ToUser(this UserDto userDto)
    {
        return new User
        {
            Id = userDto.Id,
            Name = userDto.Name,
            Email = userDto.Email,
            LastActive = userDto.LastActive,
            Interests = userDto.Interests.Select(i => i.ToInterest()).ToList(),
            FavouriteStories = userDto.FavouriteStories.Select(s => s.ToStory()).ToList(),
        };
    }
    
    public static UserDto ToUserDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            LastActive = user.LastActive,
            Interests = user.Interests.Select(i => i.ToInterestDto()).ToList(),
            FavouriteStories = user.FavouriteStories.Select(u => u.ToStoryDto()).ToList(),
        };
    }
}