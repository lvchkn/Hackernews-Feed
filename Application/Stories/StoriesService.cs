using Application.Filter;
using Application.Paging;
using Application.Sort;
using Application.Tags;
using Shared.Exceptions;

namespace Application.Stories;

public class StoriesService : IStoriesService
{
    private readonly IStoriesRepository _storiesRepository;
    public StoriesService(IStoriesRepository storiesRepository)
    {
        _storiesRepository = storiesRepository;
    }

    public async Task AddAsync(StoryDto storyDto)
    {
        var story = storyDto.ToStory();
        bool operationResult = await _storiesRepository.AddAsync(story);

        if (operationResult is false)
        {
            throw new StoryAlreadyExistsException(story.Id.ToString());
        }
    }

    public async Task<List<StoryDto>> GetAllAsync()
    {
        var stories = await _storiesRepository.GetAllAsync();
        return stories.Select(s => s.ToStoryDto()).ToList();
    }

    public async Task<List<TagDto>> GetTagsAsync(int storyId)
    {
        _ = await GetByIdAsync(storyId);
        var tags = await _storiesRepository.GetTagsAsync(storyId);
        return tags.Select(s => s.ToTagDto()).ToList();
    }

    public async Task<StoryDto> GetByIdAsync(int id)
    {
        var story = await _storiesRepository.GetByIdAsync(id);

        if (story is null)
        {
            throw new StoryNotFoundException(id.ToString());
        }

        return story.ToStoryDto();
    }

    public async Task<PagedStoriesDto> GetPagedAsync(string? orderBy, string? search, int pageNumber, int pageSize)
    {
        if (pageSize <= 0)
        {
            throw new QueryParameterException("page size cannot be less than 1");
        }

        var parsedSortingParameters = SortingParametersParser.Parse(orderBy);
        int skip = Math.Max((pageNumber - 1) * pageSize, 0);
        int take = pageSize;
        var searchCriteria = new SearchCriteria(search, 0);

        var pagedObject = await _storiesRepository.GetPagedAsync(parsedSortingParameters, searchCriteria, skip, take);

        var stories = pagedObject.Stories.Select(s => s.ToStoryDto()).ToList();

        var pagedStories = new PagedStoriesDto
        {
            Stories = stories,
            TotalPagesCount = pagedObject.TotalPagesCount,
            PageSize = pageSize,
            PageNumber = pageNumber,
        };

        // if (parsedSortingParameters.Count > 0 || !string.IsNullOrEmpty(search)) //TODO
        // {
        //     return pagedStories;
        // }

        return pagedStories;
    }
}