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

    public async Task AddInterestAsync(int id, InterestDto interestDto)
    {
        var interest = _mapper.Map<Interest>(interestDto);
        await _usersRepository.AddInterestAsync(id, interest.Id);
    }

    public async Task DeleteInterestAsync(int id, int interestId)
    {
        await _usersRepository.DeleteInterestAsync(id, interestId);
    }

    public async Task<List<UserDto>> GetAllAsync()
    {
        var users = await _usersRepository.GetAllAsync();
        return _mapper.Map<List<UserDto>>(users);
    }

    public async Task<UserDto> GetByIdAsync(int id)
    {
        var user = await _usersRepository.GetByIdAsync(id);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<List<InterestDto>> GetInterestsAsync(int id)
    {
        var userInterests = await _usersRepository.GetInterestsAsync(id);
        return _mapper.Map<List<InterestDto>>(userInterests);
    }

    public async Task UpdateLastActiveAsync(int id)
    {
        await _usersRepository.UpdateLastActiveAsync(id);
    }
}