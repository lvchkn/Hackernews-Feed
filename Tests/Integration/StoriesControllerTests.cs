using System.Net.Http.Headers;
using System.Text.Json;
using Application.Contracts;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Tests.Integration;

public class StoriesControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _webAppFactory;
    private record CreateInterestRequest(string Text, int? Id);

    public StoriesControllerTests(CustomWebApplicationFactory<Program> webAppFactory)
    {
        _webAppFactory = webAppFactory;
    }

    [Fact]
    public async Task All_stories_are_returned_when_no_query_provided()
    {
        // Arrange
        var jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var client = _webAppFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        using var scope = _webAppFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext?.SeedData();

        // Act
        var response = await client.GetAsync("/api/stories");
        var responseJson = await response.Content.ReadAsStringAsync();
        var returnedStories = JsonSerializer.Deserialize<StoryDto[]>(responseJson, jsonSerializerOptions);

        // Assert
        returnedStories.Should().NotBeNull();
        returnedStories?.Length.Should().Be(5);
    }

    [Fact]
    public async Task Stories_are_sorted_by_title_in_desc_order()
    {
        // Arrange
        var jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var client = _webAppFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        using var scope = _webAppFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext?.SeedData();

        var expectedResults = dbContext?.Stories.OrderByDescending(s => s.Title).ToList();
        
        // Act
        var response = await client.GetAsync("/api/stories?orderBy=title,asc");
        var responseJson = await response.Content.ReadAsStringAsync();
        var returnedStories = JsonSerializer.Deserialize<StoryDto[]>(responseJson, jsonSerializerOptions);

        // Assert
        returnedStories.Should().BeEquivalentTo(expectedResults);
    }
}