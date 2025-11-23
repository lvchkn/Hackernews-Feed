namespace Application.Interests;

public interface IInterestsService
{
    Task<InterestDto> GetByNameAsync(string name);
    Task<InterestDto> GetByIdAsync(int id);
    Task<List<InterestDto>> GetAllAsync();
    Task<int> AddAsync(InterestDto interest);
    Task UpdateAsync(int id, InterestDto updatedInterest);
    Task DeleteAsync(int id);
}
