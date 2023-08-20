using Application.Sort;
using AutoMapper;
using Domain.Entities;

namespace Application.Stories;

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

    public List<StoryDto> GetStories(string? orderBy, string? search, int pageNumber, int pageSize)
    {
        var parsedSortingParameters = _sortingParameteresParser.Parse(orderBy);
        var skip = (pageNumber - 1) * pageSize;
        var take = pageSize;
        
        var sortedStories = _storiesRepository.GetAll(parsedSortingParameters, search, skip, take);
        
        return _mapper.Map<List<StoryDto>>(sortedStories);
    }
}
