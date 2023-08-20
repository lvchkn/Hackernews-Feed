using System.Net.Http.Headers;
using System.Text.Json;
using Application.Stories;
using Application.Tags;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Tests.Integration;

[Collection("Custom WAF collection")]
public class StoriesControllerTests
{
    private readonly CustomWebApplicationFactory<Program> _webAppFactory;
    private record CreateInterestRequest(string Text, int? Id);

    private readonly JsonSerializerOptions _jsonSerializerOptions = new ()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public StoriesControllerTests(CustomWebApplicationFactory<Program> webAppFactory)
    {
        _webAppFactory = webAppFactory;
    }

    [Fact]
    public async Task All_stories_are_returned_when_no_query_provided()
    {
        // Arrange
        var client = _webAppFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        
        using var scope = _webAppFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext?.SeedStories();

        // Act
        var response = await client.GetAsync("/api/stories");
        var responseJson = await response.Content.ReadAsStringAsync();
        var returnedStories = JsonSerializer.Deserialize<StoryDto[]>(responseJson, _jsonSerializerOptions);

        // Assert
        returnedStories.Should().NotBeNull();
        returnedStories?.Length.Should().Be(5);
    }

    [Fact]
    public async Task Stories_are_sorted_by_title_in_desc_order()
    {
        // Arrange
        var client = _webAppFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        
        using var scope = _webAppFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext?.SeedStories();

        var expectedResults = dbContext?.Stories
            .OrderByDescending(s => s.Title)
            .Select(s => MapToStoryDto(s))
            .ToList();
        
        // Act
        var response = await client.GetAsync("/api/stories?orderBy=title,asc");
        var responseJson = await response.Content.ReadAsStringAsync();
        var returnedStories = JsonSerializer.Deserialize<StoryDto[]>(responseJson, _jsonSerializerOptions);

        // Assert
        returnedStories.Should().BeEquivalentTo(expectedResults);
    }

    [Theory]
    [InlineData("story")]
    [InlineData("sto")]
    [InlineData("or")]
    public async Task Stories_are_filtered_by_title_with_fuzzy_search(string search)
    {
        // Arrange
        var client = _webAppFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        
        using var scope = _webAppFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext?.SeedStories();
        
        // Act
        var response = await client.GetAsync($"/api/stories?search={search}");
        var responseJson = await response.Content.ReadAsStringAsync();
        var returnedStories = JsonSerializer.Deserialize<StoryDto[]>(responseJson, _jsonSerializerOptions);

        // Assert
        returnedStories?.Length.Should().Be(5);
    }

    [Theory]
    [InlineData("ory")]
    [InlineData("b")]
    [InlineData("A")]
    public async Task Stories_are_ordered_and_filtered(string search)
    {
        // Arrange
        var client = _webAppFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        using var scope = _webAppFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext?.SeedStories();
        
        var orderBy = "score asc";
        var expectedResults = dbContext?.Stories
            .Where(s => EF.Functions.ILike(s.Title, $"%{search}%"))
            .OrderBy(s => s.Score)
            .Select(s => MapToStoryDto(s))
            .ToList();
        
        // Act
        var response = await client.GetAsync($"/api/stories?orderBy={orderBy}&search={search}");
        var responseJson = await response.Content.ReadAsStringAsync();
        var returnedStories = JsonSerializer.Deserialize<StoryDto[]>(responseJson, _jsonSerializerOptions);

        // Assert
        returnedStories?.Should().BeEquivalentTo(expectedResults);
    }

    [Theory]
    [InlineData(2, 2, 2)]
    [InlineData(2, 1, 1)]
    [InlineData(4, 1, 1)]
    [InlineData(5, 0, 0)]
    [InlineData(5, 1, 1)]
    [InlineData(1, 5, 5)]
    [InlineData(3, 2, 1)]
    public async Task Pagination_Works(int pageNumber, int pageSize, int result)
    {
        // Arrange
        var client = _webAppFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        
        using var scope = _webAppFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext?.SeedStories();

        // Act
        var response = await client.GetAsync($"/api/stories?pageNumber={pageNumber}&pageSize={pageSize}");
        var responseJson = await response.Content.ReadAsStringAsync();
        var returnedStories = JsonSerializer.Deserialize<StoryDto[]>(responseJson, _jsonSerializerOptions);
        // Assert
        returnedStories?.Length.Should().Be(result);
    }

    private static StoryDto MapToStoryDto(Story story)
    {
        return new StoryDto()
        {
            By = story.By,
            Descendants = story.Descendants,
            Id = story.Id,
            Kids = story.Kids,
            Score = story.Score,
            Time = story.Time,
            Title = story.Title,
            Url = story.Url,
            Type = story.Type,
            Tags = story.Tags
                .Select(t => new TagDto
                {
                    Id = t.Id, 
                    Name = t.Name,
                })
                .ToList(),
        };
    }
}