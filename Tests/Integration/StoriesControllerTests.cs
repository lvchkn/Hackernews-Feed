using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Api;
using Application.Paging;
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

    private readonly JsonSerializerOptions _jsonSerializerOptions = new ()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public StoriesControllerTests(CustomWebApplicationFactory<Program> webAppFactory)
    {
        _webAppFactory = webAppFactory;
    }

    [Fact]
    public async Task Up_to_first_10_stories_are_returned_when_no_query_provided()
    {
        // Arrange
        var client = _webAppFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        
        using var scope = _webAppFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var storiesCount = dbContext?.SeedStories();

        var expectedStories = dbContext?.Stories
            .Select(s => MapToStoryDto(s))
            .ToList();

        // Act
        var response = await client.GetAsync("/api/stories");
        var responseJson = await response.Content.ReadAsStringAsync();
        var pagedData = JsonSerializer.Deserialize<PagedStoriesDto>(responseJson, _jsonSerializerOptions);

        // Assert
        pagedData?.Stories.Should().BeEquivalentTo(expectedStories, options => options.Excluding(s => s.Rank));
        pagedData?.Stories.Count.Should().Be(storiesCount);
    }

    [Fact]
    public async Task Stories_are_sorted_by_title_in_desc_order()
    {
        // Arrange
        var client = _webAppFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        
        using var scope = _webAppFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var storiesCount = dbContext?.SeedStories();

        var expectedStories = dbContext?.Stories
            .OrderByDescending(s => s.Title)
            .Select(s => MapToStoryDto(s))
            .ToList();
        
        // Act
        var response = await client.GetAsync("/api/stories?orderBy=title,asc");
        var responseJson = await response.Content.ReadAsStringAsync();
        var pagedData = JsonSerializer.Deserialize<PagedStoriesDto>(responseJson, _jsonSerializerOptions);

        // Assert
        pagedData?.Stories.Should().BeEquivalentTo(expectedStories, options => options.Excluding(s => s.Rank));
        pagedData?.Stories.Count.Should().Be(storiesCount);
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
        var storiesCount = dbContext?.SeedStories();

        var expectedStories = dbContext?.Stories
            .Where(s => EF.Functions.ILike(s.Title, $"%{search}%"))
            .Select(s => MapToStoryDto(s))
            .ToList();
        
        // Act
        var response = await client.GetAsync($"/api/stories?search={search}");
        var responseJson = await response.Content.ReadAsStringAsync();
        var pagedData = JsonSerializer.Deserialize<PagedStoriesDto>(responseJson, _jsonSerializerOptions);

        // Assert
        pagedData?.Stories.Should().BeEquivalentTo(expectedStories, options => options.Excluding(s => s.Rank));
        pagedData?.Stories.Count.Should().Be(expectedStories?.Count);
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
        var storiesCount = dbContext?.SeedStories();
        
        var orderBy = "score asc";
        var expectedStories = dbContext?.Stories
            .Where(s => EF.Functions.ILike(s.Title, $"%{search}%"))
            .OrderBy(s => s.Score)
            .Select(s => MapToStoryDto(s))
            .ToList();
        
        // Act
        var response = await client.GetAsync($"/api/stories?orderBy={orderBy}&search={search}");
        var responseJson = await response.Content.ReadAsStringAsync();
        var pagedData = JsonSerializer.Deserialize<PagedStoriesDto>(responseJson, _jsonSerializerOptions);

        // Assert
        pagedData?.Stories.Should().BeEquivalentTo(expectedStories, options => options.Excluding(s => s.Rank));
        pagedData?.Stories.Count.Should().Be(expectedStories?.Count);
    }

    [Theory]
    [InlineData(2, 2, 2, 3)]
    [InlineData(2, 1, 1, 5)]
    [InlineData(4, 2, 0, 3)]
    [InlineData(5, 1, 1, 5)]
    [InlineData(1, 5, 5, 1)]
    [InlineData(3, 2, 1, 3)]
    [InlineData(2, 3, 2, 2)]
    [InlineData(1, 3, 3, 2)]
    [InlineData(0, 2, 2, 3)]
    [InlineData(-1, 2, 2, 3)]
    [InlineData(20, 30, 0, 1)]
    [InlineData(20, 3, 0, 2)]
    [InlineData(-20, 3, 3, 2)]
    [InlineData(-20, 30, 5, 1)]
    public async Task Pagination_Works(int pageNumber, int pageSize, int returnedStoriesCount, int totalPagesCount)
    {
        // Arrange
        var client = _webAppFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        
        using var scope = _webAppFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var storiesCount = dbContext?.SeedStories();

        // Act
        var response = await client.GetAsync($"/api/stories?pageNumber={pageNumber}&pageSize={pageSize}");
        var responseJson = await response.Content.ReadAsStringAsync();
        var pagedData = JsonSerializer.Deserialize<PagedStoriesDto>(responseJson, _jsonSerializerOptions);
        
        // Assert
        pagedData?.Stories.Count.Should().Be(returnedStoriesCount);
        pagedData?.TotalPagesCount.Should().Be(totalPagesCount);
    }

    [Theory]
    [InlineData(2, 0)]
    [InlineData(0, 0)]
    [InlineData(2, -1)]
    [InlineData(-1, -1)]
    public async Task Pagination_Edge_Cases(int pageNumber, int pageSize)
    {
        // Arrange
        var client = _webAppFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        
        using var scope = _webAppFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var storiesCount = dbContext?.SeedStories();

        // Act
        var response = await client.GetAsync($"/api/stories?pageNumber={pageNumber}&pageSize={pageSize}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
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
            Text = story.Text,
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