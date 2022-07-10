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

        public Task<string> AddAsync(InterestDto interestDto)
        {
            var interest = _mapper.Map<Interest>(interestDto);
            return _interestsRepository.AddAsync(interest);
        }

        public Task<string> DeleteAsync(string id)
        {
            return _interestsRepository.DeleteAsync(id);
        }

        public async Task<List<InterestDto>> GetAllAsync()
        {
            var interests = await _interestsRepository.GetAllAsync();
            return _mapper.Map<List<InterestDto>>(interests);
        }

        public async Task<InterestDto> GetByIdAsync(string id)
        {
            var interest = await _interestsRepository.GetByIdAsync(id);
            return _mapper.Map<InterestDto>(interest);
        }

        public Task<string> UpdateAsync(string id, InterestDto updatedInterest)
        {
            var interest = _mapper.Map<Interest>(updatedInterest);
            return _interestsRepository.UpdateAsync(id, interest);
        }
    }
}
