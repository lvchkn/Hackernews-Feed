using Application.Contracts;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Services.Stories;

public class StoriesService : IStoriesService
{
    private readonly IStoriesRepository _storiesRepository;
    private readonly IMapper _mapper;

    public StoriesService(IStoriesRepository storiesRepository, IMapper mapper)
    {
        _storiesRepository = storiesRepository;
        _mapper = mapper;
    }

    public async Task AddAsync(StoryDto storyDto)
    {
        var story = _mapper.Map<Story>(storyDto);
        await _storiesRepository.AddAsync(story);
    }

    public async Task<List<StoryDto>> GetAllAsync()
    {
        var stories = await _storiesRepository.GetAllAsync();
        return _mapper.Map<List<StoryDto>>(stories);
    }
}
