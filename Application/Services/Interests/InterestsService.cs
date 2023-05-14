using Application.Contracts;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Services.Interests
{
    public class InterestsService : IInterestsService
    {
        private readonly IMapper _mapper;
        private readonly IInterestsRepository _interestsRepository;

        public InterestsService(IMapper mapper, IInterestsRepository interestsRepository)
        {
            _mapper = mapper;
            _interestsRepository = interestsRepository;
        }

        public async Task<int> AddAsync(InterestDto interestDto)
        {
            var interest = _mapper.Map<Interest>(interestDto);
            var id = await _interestsRepository.AddAsync(interest);

            return id;
        }

        public async Task DeleteAsync(int id)
        {
            await _interestsRepository.DeleteAsync(id);
        }

        public async Task<List<InterestDto>> GetAllAsync()
        {
            var interests = await _interestsRepository.GetAllAsync();
            return _mapper.Map<List<InterestDto>>(interests);
        }

        public async Task<InterestDto> GetByNameAsync(string name)
        {
            var interest = await _interestsRepository.GetByNameAsync(name);
            return _mapper.Map<InterestDto>(interest);
        }

        public async Task<InterestDto> GetByIdAsync(int id)
        {
            var interest = await _interestsRepository.GetByIdAsync(id);
            return _mapper.Map<InterestDto>(interest);
        }

        public async Task UpdateAsync(int id, InterestDto updatedInterest)
        {
            var interest = _mapper.Map<Interest>(updatedInterest);
            await _interestsRepository.UpdateAsync(id, interest);
        }
    }
}
