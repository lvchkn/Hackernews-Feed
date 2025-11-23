using Application.Interests;
using Application.Mappers;
using Shared.Exceptions;

namespace Application.Users;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;

    public UsersService(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }
        
    public async Task AddAsync(UserDto userDto)
    {
        var user = userDto.ToUser();
        await _usersRepository.AddAsync(user);
    }

    public async Task AddInterestAsync(int id, InterestDto interestDto)
    {
        var interest = interestDto.ToInterest();
        bool result = await _usersRepository.AddInterestAsync(id, interest.Id);

        if (result == false)
        {
            throw new EntityNotFoundException($"User id: {id} Interest id: {interest.Id}");
        }
    }

    public async Task DeleteInterestAsync(int id, int interestId)
    {
        var result = await _usersRepository.DeleteInterestAsync(id, interestId);
        
        if (result == false)
        {
            throw new EntityNotFoundException($"User id: {id} Interest id: {interestId}");
        }
    }

    public async Task<List<UserDto>> GetAllAsync()
    {
        var users = await _usersRepository.GetAllAsync();
        return users.Select(u => u.ToUserDto()).ToList();
    }

    public async Task<UserDto> GetByIdAsync(int id)
    {
        var user = await _usersRepository.GetByIdAsync(id);

        if (user is null)
        {
            throw new UserNotFoundException(id.ToString());
        }

        return user.ToUserDto();
    }

    public async Task<List<InterestDto>> GetInterestsAsync(int id)
    {
        var userInterests = await _usersRepository.GetInterestsAsync(id);
        
        if (userInterests is null)
        {
            throw new UserNotFoundException(id.ToString());
        }
        
        return userInterests.Select(u => u.ToInterestDto()).ToList();
    }

    public async Task UpdateLastActiveAsync(int id)
    {
        bool result = await _usersRepository.UpdateLastActiveAsync(id);
        
        if (result == false)
        {
            throw new UserNotFoundException(id.ToString());
        }
    }
}