using Application.Interests;
using AutoMapper;
using Domain.Entities;

namespace Application.Users;

public class UsersService : IUsersService
{
    private readonly IMapper _mapper;
    private readonly IUsersRepository _usersRepository;

    public UsersService(IMapper mapper, IUsersRepository usersRepository)
    {
        _mapper = mapper;
        _usersRepository = usersRepository;
    }
        
    public async Task AddAsync(UserDto userDto)
    {
        var user = _mapper.Map<User>(userDto);
        await _usersRepository.AddAsync(user);
    }

    public async Task AddInterestAsync(string email, InterestDto interestDto)
    {
        var interest = _mapper.Map<Interest>(interestDto);
        await _usersRepository.AddInterestAsync(email, interest.Id);
    }

    public async Task DeleteInterestAsync(string email, int interestId)
    {
        await _usersRepository.DeleteInterestAsync(email, interestId);
    }

    public async Task<List<UserDto>> GetAllAsync()
    {
        var users = await _usersRepository.GetAllAsync();
        return _mapper.Map<List<UserDto>>(users);
    }

    public async Task<UserDto> GetByEmailAsync(string email)
    {
        var user = await _usersRepository.GetByEmailAsync(email);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<List<InterestDto>> GetInterestsAsync(string email)
    {
        var userInterests = await _usersRepository.GetInterestsAsync(email);
        return _mapper.Map<List<InterestDto>>(userInterests);
    }

    public async Task UpdateLastActiveAsync(string email)
    {
        await _usersRepository.UpdateLastActiveAsync(email);
    }
}