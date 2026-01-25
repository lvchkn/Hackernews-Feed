using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Application.Paging;
using Application.Tags;
using Domain.Entities;
using Infrastructure.Db;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Tests.Integration;

[Collection("Test server collection")]
public class TagsControllerTests
{
    private readonly TestServerFixture<Program> _webAppFactory;

    public TagsControllerTests(TestServerFixture<Program> webAppFactory)
    {
        _webAppFactory = webAppFactory;
    }

    [Fact]
    public async Task GetAllTags_ReturnsSeededTags()
    {
        // Arrange
        var client = _webAppFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        using var scope = _webAppFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.SeedTags();

        // Act
        var pageNumber = 1;
        var pageSize = 5;
        var response = await client.GetAsync($"/api/tags?pageNumber={pageNumber}&pageSize={pageSize}", TestContext.Current.CancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        var pagedTags = await response.Content.ReadFromJsonAsync<PagedTagsDto>(TestContext.Current.CancellationToken);

        Assert.NotNull(pagedTags);
        Assert.NotEmpty(pagedTags.Tags);
        Assert.Equal(1, pagedTags.TotalPagesCount);

        Assert.Contains(pagedTags.Tags, t =>
            t.Name == "PostgreSQL"
            && t.Category == "Databases"
            && t.Aliases.Exists(a => a.Name == "Postgres"));

        Assert.Contains(pagedTags.Tags, t =>
            t.Name == ".NET"
            && t.Category == "Frameworks"
            && t.Aliases.Exists(a => a.Name == "dotnet"));
    }

    [Fact]
    public async Task GetStoryTags_ReturnsTagsForStory()
    {
        // Arrange
        var client = _webAppFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        using var scope = _webAppFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        dbContext.Stories.RemoveRange(dbContext.Stories);
        dbContext.Tags.RemoveRange(dbContext.Tags);
        await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var expectedTagName = "TestTag";
        var expectedTagCategory = "TestCategory";
        var tag = new Tag { Name = expectedTagName, Category = expectedTagCategory };
        var story = new Story
        {
            Id = 1,
            Title = "Test Story",
            By = "testuser",
            Score = 10,
            Time = 1600000000,
            Tags = [tag]
        };

        dbContext.Stories.Add(story);
        await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var response = await client.GetAsync($"/api/stories/{story.Id}/tags", TestContext.Current.CancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        var tags = await response.Content.ReadFromJsonAsync<List<TagDto>>(TestContext.Current.CancellationToken);

        Assert.NotNull(tags);
        Assert.Single(tags);
        Assert.Equal(expectedTagName, tags[0].Name);
        Assert.Equal(expectedTagCategory, tags[0].Category);
    }

    [Fact]
    public async Task GetStoryTags_NonExistentStory_ReturnsNotFound()
    {
        // Arrange
        var client = _webAppFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        // Act
        var response = await client.GetAsync("/api/stories/9999/tags", TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
