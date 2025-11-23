using Application.Sort;
using Domain.Entities;
using Infrastructure.Db;
using JetBrains.Annotations;
using Xunit;

namespace Tests.Unit;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class UtilsTests
{
    private static IQueryable<Story> Stories => new List<Story>
    {
        new() { Id = 1, Title = "D Story", Score = 24 },
        new() { Id = 2, Title = "C Story", Score = 10 },
        new() { Id = 3, Title = "B Story", Score = 5 },
        new() { Id = 4, Title = "A Story", Score = 42 },
    }.AsQueryable();

    [Theory]
    [InlineData(SortField.Id)]
    [InlineData(SortField.Score)]
    [InlineData(SortField.Title)]
    public void Sorting_by_fields_in_ascending_order_works(SortField field)
    {
        // Arrange
        var sortingParameters = new List<SortParameters>()
        {
            new(SortOrder.Asc, field),
        };
        
        // Act
        var sortedStories = new StorySort().Sort(Stories, sortingParameters);
        
        // Assert
        if (field == SortField.Id)
        {
            Assert.Equivalent(Stories.OrderBy(s => s.Id), sortedStories);
        }
        else if (field == SortField.Title)
        {
            Assert.Equivalent(Stories.OrderBy(s => s.Title), sortedStories);
        }
        else 
        {
            Assert.Equivalent(Stories.OrderBy(s => s.Score), sortedStories);
        }
    }
    
    [Theory]
    [InlineData(SortField.Id)]
    [InlineData(SortField.Score)]
    [InlineData(SortField.Title)]
    public void Sorting_by_fields_in_descending_order_works(SortField field)
    {
        // Arrange
        var sortingParameters = new List<SortParameters>
        {
            new(SortOrder.Desc, field),
        };
        
        // Act
        var sortedStories = new StorySort().Sort(Stories, sortingParameters);
        
        // Assert
        if (field == SortField.Id)
        {
            Assert.Equivalent(Stories.OrderByDescending(s => s.Id), sortedStories);
        }
        else if (field == SortField.Title)
        {
            Assert.Equivalent(Stories.OrderByDescending(s => s.Title), sortedStories);
        }
        else 
        {
            Assert.Equivalent(Stories.OrderByDescending(s => s.Score), sortedStories);
        }   
    }
    
    [Fact]
    public void Chained_sort_works()
    {
        // Arrange
        var sortingParameters = new List<SortParameters>()
        {
            new(SortOrder.Desc, SortField.Score),
            new(SortOrder.Asc, SortField.Title),
        };
        
        var expected = Stories.OrderByDescending(c => c.Score).ThenBy(c => c.Title).ToList();

        var sorter = new StorySort();
        
        // Act
        var sortedStories = sorter.Sort(Stories, sortingParameters);
        
        // Assert
        Assert.Equivalent(expected, sortedStories);
    }
}