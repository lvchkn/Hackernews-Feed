using Application.Contracts;

namespace Application.Services.Interests
{
    public interface IInterestsService
    {
        Task<InterestDto> GetByIdAsync(string id);
        Task<List<InterestDto>> GetAllAsync();
        Task<string> AddAsync(InterestDto interest);
        Task<string> UpdateAsync(string id, InterestDto updatedInterest);
        Task<string> DeleteAsync(string id);
    }
}
