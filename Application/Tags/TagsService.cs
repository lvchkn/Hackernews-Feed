using Application.Paging;

namespace Application.Tags;

public class TagsService : ITagsService
{
    private readonly ITagsRepository _tagsRepository;

    public TagsService(ITagsRepository tagsRepository)
    {
        _tagsRepository = tagsRepository;
    }

    public async Task<List<TagDto>> GetAllAsync()
    {
        var tags = await _tagsRepository.GetAllAsync();
        return tags.Select(t => t.ToTagDto()).ToList();
    }

    public async Task<PagedTagsDto> GetPagedAsync(int pageNumber, int pageSize)
    {
        int skip = Math.Max((pageNumber - 1) * pageSize, 0);
        int take = pageSize;
        var (tags, totalPagesCount) = await _tagsRepository.GetPagedAsync(skip, take);

        return new PagedTagsDto
        {
            Tags = tags.Select(t => t.ToTagDto()).ToList(),
            TotalPagesCount = totalPagesCount
        };
    }

    public async Task SeedAsync()
    {
        if (!await _tagsRepository.ExistsAsync())
        {
            var seedData = TagsSeeder.GetSeedTags();
            await _tagsRepository.AddRangeAsync(seedData);
        }
    }
}