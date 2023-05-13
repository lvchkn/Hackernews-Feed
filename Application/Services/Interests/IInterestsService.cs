using Application.Contracts;

namespace Application.Services.Interests
{
    public interface IInterestsService
    {
        Task<InterestDto> GetByNameAsync(string name);
        Task<List<InterestDto>> GetAllAsync();
        Task AddAsync(InterestDto interest);
        Task UpdateAsync(int id, InterestDto updatedInterest);
        Task DeleteAsync(int id);
    }
}
