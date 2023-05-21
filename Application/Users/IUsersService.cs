using Application.Interests;

namespace Application.Users;

public interface IUsersService
{
    Task<List<UserDto>> GetAllAsync();
    Task<UserDto> GetByEmailAsync(string email);
    Task UpdateLastActiveAsync(string email);
    Task AddInterestAsync(string email, InterestDto interest);
    Task AddAsync(UserDto user);
    Task DeleteInterestAsync(string email, int interestId);
    Task<List<InterestDto>> GetInterestsAsync(string email);
}
