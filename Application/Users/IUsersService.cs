using Application.Interests;

namespace Application.Users;

public interface IUsersService
{
    Task<List<UserDto>> GetAllAsync();
    Task<UserDto> GetByIdAsync(int id);
    Task UpdateLastActiveAsync(int id);
    Task AddInterestAsync(int id, InterestDto interest);
    Task AddAsync(UserDto user);
    Task DeleteInterestAsync(int id, int interestId);
    Task<List<InterestDto>> GetInterestsAsync(int id);
}
