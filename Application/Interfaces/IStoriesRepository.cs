using Domain.Entities;

namespace Application.Interfaces;

public interface IStoriesRepository
{
    Task<string> AddAsync(Story story);
}