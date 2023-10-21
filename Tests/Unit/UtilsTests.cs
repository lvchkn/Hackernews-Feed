using System.Text.Json;
using Application.Sort;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Db;
using Shared.Utils;
using Xunit;

namespace Tests.Unit;

public class UtilsTests
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    
    private IQueryable<Story> Stories => new List<Story>
    {
        new() { Id = 1, Title = "D Story", Score = 24 },
        new() { Id = 2, Title = "C Story", Score = 10 },
        new() { Id = 3, Title = "B Story", Score = 5 },
        new() { Id = 4, Title = "A Story", Score = 42 },
    }.AsQueryable();
    
    [Fact]
    public void Item_type_is_inferred_correctly()
    {
        // Arrange
        var story = new Story
        {
            Title = "Something",
            Type = "story"
        };
        var item = JsonSerializer.Serialize(story, _jsonSerializerOptions);

        // Act
        var type = ItemUtils.GetItemType(item);

        // Assert
        type.Should().Be("story");
    }

    [Theory]
    [InlineData(SortField.Id)]
    [InlineData(SortField.Score)]
    [InlineData(SortField.Title)]
    public void Sorting_by_fields_in_ascending_order_works(SortField field)
    {
        // Arrange
        var sortingParameters = new List<SortingParameters>()
        {
            new(SortOrder.Asc, field),
        };
        
        // Act
        var sortedStories = new Sorter().Sort(Stories, sortingParameters);
        
        // Assert
        if (field == SortField.Id)
        {
            sortedStories.Should().BeInAscendingOrder(c => c.Id);
        }
        else if (field == SortField.Title)
        {
            sortedStories.Should().BeInAscendingOrder(c => c.Title);
        }
        else 
        {
            sortedStories.Should().BeInAscendingOrder(c => c.Score);
        }
    }
    
    [Theory]
    [InlineData(SortField.Id)]
    [InlineData(SortField.Score)]
    [InlineData(SortField.Title)]
    public void Sorting_by_fields_in_descending_order_works(SortField field)
    {
        // Arrange
        var sortingParameters = new List<SortingParameters>
        {
            new(SortOrder.Desc, field),
        };
        
        // Act
        var sortedStories = new Sorter().Sort(Stories, sortingParameters);
        
        // Assert
        if (field == SortField.Id)
        {
            sortedStories.Should().BeInDescendingOrder(c => c.By);
        }
        else if (field == SortField.Title)
        {
            sortedStories.Should().BeInDescendingOrder(c => c.Title);
        }
        else 
        {
            sortedStories.Should().BeInDescendingOrder(c => c.Score);
        }   
    }
    
    [Fact]
    public void Chained_sort_works()
    {
        // Arrange
        var sortingParameters = new List<SortingParameters>()
        {
            new(SortOrder.Desc, SortField.Score),
            new(SortOrder.Asc, SortField.Title),
        };
        
        var expected = Stories.OrderByDescending(c => c.Score).ThenBy(c => c.Title).ToList();

        var sorter = new Sorter();
        
        // Act
        var sortedStories = sorter.Sort(Stories, sortingParameters);
        
        // Assert
        sortedStories.Should().BeEquivalentTo(expected);
    }
}