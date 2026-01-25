using System.Text.RegularExpressions;
using Application.Paging;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Tags;

public class TagsService : ITagsService
{
    private readonly ITagsRepository _tagsRepository;
    private readonly ILogger<TagsService> _logger;

    public TagsService(ITagsRepository tagsRepository,
        ILogger<TagsService> logger)
    {
        _tagsRepository = tagsRepository;
        _logger = logger;
    }

    public async Task<List<TagDto>> GetAllAsync()
    {
        var tags = await _tagsRepository.GetAllAsync();
        return tags.Select(t => t.ToTagDto()).ToList();
    }

    public async Task<List<TagDto>> GetTagsForTextAsync(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return [];
        }

        var allTags = await _tagsRepository.GetAllAsync();
        var matchedTags = new HashSet<Tag>();

        foreach (var tag in allTags)
        {
            if (IsWordMatch(text, tag.Name))
            {
                matchedTags.Add(tag);
                continue;
            }

            foreach (var alias in tag.Aliases)
            {
                if (IsWordMatch(text, alias.Name))
                {
                    matchedTags.Add(tag);
                    break;
                }
            }
        }

        return matchedTags.Select(tag => tag.ToTagDto()).ToList();
    }

    private static bool IsWordMatch(string text, string keyword)
    {
        string escapedKeyword = Regex.Escape(keyword);
        var pattern = $@"(?<!\w){escapedKeyword}(?!\w)";
        return Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase);
    }

    public async Task<PagedTagsDto> GetPagedAsync(int pageNumber, int pageSize)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(pageNumber, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

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
        _logger.LogInformation("Seeding tags...");
        var existingTags = await _tagsRepository.GetAllAsync();
        var existingTagsMap = existingTags
            .GroupBy(t => t.Name, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);

        List<Tag> seedTags = TagsSeeder.GetSeedTags();
        var tagsToAdd = new List<Tag>();
        var hasChanges = false;

        foreach (var seedTag in seedTags)
        {
            if (existingTagsMap.TryGetValue(seedTag.Name, out var existingTag))
            {
                var existingAliases = existingTag.Aliases
                    .Select(a => a.Name)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                var missingSeedAliases = seedTag.Aliases.Where(seedAlias => !existingAliases.Contains(seedAlias.Name));

                foreach (var seedAlias in missingSeedAliases)
                {
                    existingTag.Aliases.Add(new TagAlias { Name = seedAlias.Name });
                    hasChanges = true;
                }
            }
            else
            {
                tagsToAdd.Add(seedTag);
            }
        }

        if (tagsToAdd.Count != 0)
        {
            await _tagsRepository.AddRangeAsync(tagsToAdd);
            await _tagsRepository.SaveChangesAsync();
            _logger.LogInformation("Tags added: {0}.", tagsToAdd.Count);
        }
        else if (hasChanges)
        {
            int changes = await _tagsRepository.SaveChangesAsync();
            _logger.LogInformation("Tags updated: {0}.", changes);
        }
        else
        {
            _logger.LogInformation("No tags added or updated.");
        }
    }
}