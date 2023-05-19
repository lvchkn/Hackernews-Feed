using Application.Contracts;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Services.Stories;

public class StoriesService : IStoriesService
{
    private readonly IStoriesRepository _storiesRepository;
    private readonly IMapper _mapper;
    private readonly SortingParametersParser _sortingParameteresParser;

    public StoriesService(
        IStoriesRepository storiesRepository, 
        IMapper mapper, 
        SortingParametersParser sortingParameteresParser)
    {
        _storiesRepository = storiesRepository;
        _mapper = mapper;
        _sortingParameteresParser = sortingParameteresParser;
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

    public List<StoryDto> GetSortedStories(string? query)
    {
        var parsedSortingParameters = _sortingParameteresParser.Parse(query);

        var sortedStories = _storiesRepository.GetSortedStories(parsedSortingParameters);
        return _mapper.Map<List<StoryDto>>(sortedStories);
    }
}
