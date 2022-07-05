using System.Text.Json;
using Application.Services;
using Domain.Entities;
using FluentAssertions;
using HackerNewsCommentsFeed.Domain;
using HackerNewsCommentsFeed.Utils;
using Xunit;

namespace Tests;

public class UtilsTests
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    
    private List<Comment> Comments => new()
    {
        new() {By = "A random person", Text = "What is going on?"},
        new() {By = "Christopher Robin", Text = "Actions speak louder than words"},
        new() {By = "James Hetfield", Text = "Boo!"},
        new() {By = "Albert Einstein", Text = "E=MC2"},
    };
    
    [Fact]
    public void Item_type_is_inferred_correctly()
    {
        // Arrange
        var comment = new Comment
        {
            Text = "Something",
            Type = "comment"
        };
        var item = JsonSerializer.Serialize(comment, _jsonSerializerOptions);

        // Act
        var type = ItemUtils.GetItemType(item);

        // Assert
        type.Should().Be("comment");
    }

    [Theory]
    [InlineData(SortField.By)]
    [InlineData(SortField.Text)]
    public void Sorting_by_fields_in_ascending_order_works(SortField field)
    {
        // Arrange
        var sortingParameters = new SortingParameters(SortOrder.Asc, field);
        
        // Act
        var sortedComments = new Sorter().Sort(Comments, sortingParameters);
        
        // Assert
        if (field == SortField.By)
        {
            sortedComments.Should().BeInAscendingOrder(c => c.By);
        }
        else 
        {
            sortedComments.Should().BeInAscendingOrder(c => c.Text);
        }    
    }
    
    [Theory]
    [InlineData(SortField.By)]
    [InlineData(SortField.Text)]
    public void Sorting_by_fields_in_descending_order_works(SortField field)
    {
        // Arrange
        var sortingParameters = new SortingParameters(SortOrder.Desc, field);
        
        // Act
        var sortedComments = new Sorter().Sort(Comments, sortingParameters);
        
        // Assert
        if (field == SortField.By)
        {
            sortedComments.Should().BeInDescendingOrder(c => c.By);
        }
        else 
        {
            sortedComments.Should().BeInDescendingOrder(c => c.Text);
        }
    }
    
    [Fact]
    public void Chained_sort_works()
    {
        // Arrange
        var firstSortingParameters = new SortingParameters(SortOrder.Desc, SortField.By);
        var secondSortingParameters = new SortingParameters(SortOrder.Asc, SortField.Text);
        var sorter = new Sorter();
        
        // Act
        var firstSorting = sorter.Sort(Comments, firstSortingParameters);
        var secondSorting = sorter.Sort(Comments, secondSortingParameters);
        
        // Assert
        var expected = Comments.OrderByDescending(c => c.By).ThenBy(c => c.Text).ToList();
        secondSorting.Should().BeEquivalentTo(expected);
    }
}