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

    public Task<string> AddAsync(StoryDto storyDto)
    {
        var story = _mapper.Map<Story>(storyDto);
        return _storiesRepository.AddAsync(story);
    }
}
