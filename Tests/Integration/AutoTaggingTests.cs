using Application.Stories;
using Application.Tags;
using Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Tests.Integration;

[Collection("Test server collection")]
public class AutoTaggingTests
{
    private readonly TestServerFixture<Program> _webAppFactory;

    public AutoTaggingTests(TestServerFixture<Program> webAppFactory)
    {
        _webAppFactory = webAppFactory;
    }

    [Fact]
    public async Task AddAsync_DetectsTagsByCanonicalName()
    {
        // Arrange
        using var scope = _webAppFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var storiesService = scope.ServiceProvider.GetRequiredService<IStoriesService>();
        var tagsService = scope.ServiceProvider.GetRequiredService<ITagsService>();

        await tagsService.SeedAsync();

        var storyDto = new StoryDto
        {
            Title = "Learning Rust and Go programming languages",
            By = "user",
            Url = "http://example.com"
        };

        // Act
        await storiesService.AddAsync(storyDto);

        // Assert
        var story = await dbContext.Stories
            .Include(s => s.Tags)
            .FirstOrDefaultAsync(s => s.Title == storyDto.Title,
                cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(story);
        Assert.Contains(story.Tags, t => t.Name == "Rust");
        Assert.Contains(story.Tags, t => t.Name == "Go");
        Assert.DoesNotContain(story.Tags, t => t.Name == "Programming Languages");
        Assert.Contains(story.Tags, t => t.Category == "Programming Languages");
    }

    [Fact]
    public async Task AddAsync_DetectsTagsByAlias()
    {
        // Arrange
        using var scope = _webAppFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var storiesService = scope.ServiceProvider.GetRequiredService<IStoriesService>();
        var tagsService = scope.ServiceProvider.GetRequiredService<ITagsService>();

        await tagsService.SeedAsync();

        var storyDto = new StoryDto
        {
            Title = "New features in dotnet 9 release",
            By = "user"
        };

        // Act
        await storiesService.AddAsync(storyDto);

        // Assert
        var story = await dbContext.Stories
            .Include(s => s.Tags)
            .FirstOrDefaultAsync(s => s.Title == storyDto.Title,
                cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(story);
        Assert.Contains(story.Tags, t => t.Name == ".NET");
        Assert.Contains(story.Tags, t => t.Category == "Frameworks");
    }

    [Fact]
    public async Task AddAsync_DoesNotMatchPartialWords()
    {
        // Arrange
        using var scope = _webAppFactory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var storiesService = scope.ServiceProvider.GetRequiredService<IStoriesService>();
        var tagsService = scope.ServiceProvider.GetRequiredService<ITagsService>();

        await tagsService.SeedAsync();

        var storyDto = new StoryDto
        {
            Title = "Google announces new search features",
            By = "user"
        };

        // Act
        await storiesService.AddAsync(storyDto);

        // Assert
        var story = await dbContext.Stories
            .Include(s => s.Tags)
            .FirstOrDefaultAsync(s => s.Title == storyDto.Title,
                cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(story);
        Assert.Contains(story.Tags, t => t.Name == "Google");
        Assert.Contains(story.Tags, t => t.Category == "Companies");
        Assert.DoesNotContain(story.Tags, t => t.Name == "Go");
    }

    [Fact]
    public async Task AddAsync_MatchesSpecialCharacters_Cpp_CSharp()
    {
        // Arrange
        using var scope = _webAppFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var storiesService = scope.ServiceProvider.GetRequiredService<IStoriesService>();
        var tagsService = scope.ServiceProvider.GetRequiredService<ITagsService>();

        await tagsService.SeedAsync();

        var storyDto = new StoryDto
        {
            Title = "Comparison of C++ and C# performance",
            By = "user"
        };

        // Act
        await storiesService.AddAsync(storyDto);

        // Assert
        var story = await dbContext.Stories
            .Include(s => s.Tags)
            .FirstOrDefaultAsync(s => s.Title == storyDto.Title,
                cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(story);
        Assert.Contains(story.Tags, t => t.Name == "C++");
        Assert.Contains(story.Tags, t => t.Name == "C#");
    }
}
