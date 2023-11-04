using Application.Paging;
using Application.Sort;
using AutoMapper;
using Domain.Entities;

namespace Application.Stories;

public class StoriesService : IStoriesService
{
    private readonly IStoriesRepository _storiesRepository;
    private readonly IMapper _mapper;
    private readonly IRankingService _rankingService;
    private readonly SortingParametersParser _sortingParametersParser;

    public StoriesService(
        IStoriesRepository storiesRepository,
        IMapper mapper,
        SortingParametersParser sortingParametersParser,
        IRankingService rankingService)
    {
        _storiesRepository = storiesRepository;
        _mapper = mapper;
        _sortingParametersParser = sortingParametersParser;
        _rankingService = rankingService;
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

    public PagedStoriesDto GetStories(string? orderBy, string? search, int pageNumber, int pageSize)
    {
        var parsedSortingParameters = _sortingParametersParser.Parse(orderBy);
        var skip = (pageNumber - 1) * pageSize;
        var take = pageSize;
        
        var sortedStories = _storiesRepository.GetAll(parsedSortingParameters, search);
        
        var dtos = _mapper.Map<List<StoryDto>>(sortedStories);
        PagedStoriesDto pagedStories;
        
        if (parsedSortingParameters.Count > 0 || !string.IsNullOrEmpty(search))
        {
            pagedStories = dtos.Paginate(skip, take);
            return pagedStories;
        }

        var rankedStories = _rankingService.Rank(dtos);
        pagedStories = rankedStories.Paginate(skip, take);
        
        return pagedStories;
    }
}