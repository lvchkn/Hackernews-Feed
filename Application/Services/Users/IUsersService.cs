using Application.Contracts;

namespace Application.Services.Users
{
    public interface IUsersService
    {
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto> GetByEmailAsync(string email);
        Task<string> UpdateLastActiveAsync(string email);
        Task<string> UpdateInterestsAsync(string email, IEnumerable<string> interestIds);
        Task<string> AddInterestAsync(string email, InterestDto interest);
        Task<string> AddAsync(UserDto user);
        Task<string> DeleteInterestAsync(string email, string interestId);
        Task<List<string>> GetInterestsNamesAsync(string email);
        Task<List<string>> GetInterestsAsync(string email);
    }
}
