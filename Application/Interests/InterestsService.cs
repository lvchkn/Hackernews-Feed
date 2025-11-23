using Application.Mappers;
using Shared.Exceptions;

namespace Application.Interests;

public class InterestsService : IInterestsService
{
    private readonly IInterestsRepository _interestsRepository;

    public InterestsService(IInterestsRepository interestsRepository)
    {
        _interestsRepository = interestsRepository;
    }

    public async Task<int> AddAsync(InterestDto interestDto)
    {
        var interest = interestDto.ToInterest();
        var id = await _interestsRepository.AddAsync(interest);

        if (id is null)
        {
            throw new InterestAlreadyExistsException(interestDto.Text);
        }

        return id.Value;
    }

    public async Task DeleteAsync(int id)
    {
        bool result = await _interestsRepository.DeleteAsync(id);

        if (result == false)
        {
            throw new InterestNotFoundException(id.ToString());
        }
    }

    public async Task<List<InterestDto>> GetAllAsync()
    {
        var interests = await _interestsRepository.GetAllAsync();
        return interests.Select<Domain.Entities.Interest, InterestDto>(i => i.ToInterestDto()).ToList();
    }

    public async Task<InterestDto> GetByNameAsync(string name)
    {
        var interest = await _interestsRepository.GetByNameAsync(name);
        return interest?.ToInterestDto()
            ?? throw new InterestNotFoundException(name);
    }

    public async Task<InterestDto> GetByIdAsync(int id)
    {
        var interest = await _interestsRepository.GetByIdAsync(id);
        return interest?.ToInterestDto()
            ?? throw new InterestNotFoundException(id.ToString());
    }

    public async Task UpdateAsync(int id, InterestDto updatedInterestDto)
    {
        var interest = updatedInterestDto.ToInterest();

        if (interest is null)
        {
            throw new InterestNotFoundException(updatedInterestDto.Text);
        }
        
        await _interestsRepository.UpdateAsync(id, interest);
    }
}