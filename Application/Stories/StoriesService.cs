using Application.Paging;
using Application.Sort;
using Application.Tags;
using AutoMapper;
using Domain.Entities;
using Shared.Exceptions;

namespace Application.Stories;

public class StoriesService : IStoriesService
{
    private readonly IStoriesRepository _storiesRepository;
    private readonly IMapper _mapper;
    private readonly IRankingService _rankingService;
    private readonly SortingParametersParser _sortingParametersParser;
    private readonly ITagsService _tagsService;
    
    public StoriesService(
        IStoriesRepository storiesRepository,
        IMapper mapper,
        SortingParametersParser sortingParametersParser,
        IRankingService rankingService,
        ITagsService tagsService)
    {
        _storiesRepository = storiesRepository;
        _mapper = mapper;
        _sortingParametersParser = sortingParametersParser;
        _rankingService = rankingService;
        _tagsService = tagsService;
    }

    public async Task AddAsync(StoryDto storyDto)
    {
        var tags = _tagsService.GetAll(storyDto);
        
        var storyWithTags = storyDto with
        {
            Tags = tags,
        };
        
        var story = _mapper.Map<Story>(storyWithTags);
        await _storiesRepository.AddAsync(story);
    }

    public async Task<List<StoryDto>> GetAllAsync()
    {
        var stories = await _storiesRepository.GetAllAsync();
        return _mapper.Map<List<StoryDto>>(stories);
    }

    public PagedStoriesDto GetStories(string? orderBy, string? search, int pageNumber, int pageSize)
    {
        if (pageSize <= 0)
        {
            throw new QueryParameterException("page size cannot be less than 1");
        }
        
        var parsedSortingParameters = _sortingParametersParser.Parse(orderBy);
        var skip = Math.Max((pageNumber - 1) * pageSize, 0);
        var take = pageSize;
        
        var (sortedStories, totalPagesCount) = _storiesRepository.GetAll(parsedSortingParameters, search, skip, take);
        
        var dtos = _mapper.Map<List<StoryDto>>(sortedStories);
        var pagedStories = new PagedStoriesDto()
        {
            Stories = dtos,
            TotalPagesCount = totalPagesCount,
        };
        
        if (parsedSortingParameters.Count > 0 || !string.IsNullOrEmpty(search)) //TODO
        {
            return pagedStories;
        }

        var rankedStories = _rankingService.Rank(pagedStories.Stories);
        pagedStories = pagedStories with
        {
            Stories = rankedStories,
        };
        
        return pagedStories;
    }
}