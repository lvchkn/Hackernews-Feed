using Application.Contracts;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly IMapper _mapper;
        private readonly IUsersRepository _usersRepository;

        public UsersService(IMapper mapper, IUsersRepository usersRepository)
        {
            _mapper = mapper;
            _usersRepository = usersRepository;
        }
        
        public Task<string> AddAsync(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            return _usersRepository.AddAsync(user);
        }

        public Task<string> AddInterestAsync(string email, InterestDto interestDto)
        {
            var interest = _mapper.Map<Interest>(interestDto);
            return _usersRepository.AddInterestAsync(email, interest);
        }

        public Task<string> DeleteInterestAsync(string email, string interestId)
        {
            return _usersRepository.DeleteInterestAsync(email, interestId);
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

        public Task<List<string>> GetInterestsAsync(string email)
        {
            return _usersRepository.GetInterestsAsync(email);
        }

        public Task<List<string>> GetInterestsNamesAsync(string email)
        {
            return _usersRepository.GetInterestsNamesAsync(email);
        }

        public Task<string> UpdateInterestsAsync(string email, IEnumerable<string> interestIds)
        {
            return _usersRepository.UpdateInterestsAsync(email, interestIds);
        }

        public Task<string> UpdateLastActiveAsync(string email)
        {
            return _usersRepository.UpdateLastActiveAsync(email);
        }
    }
}
