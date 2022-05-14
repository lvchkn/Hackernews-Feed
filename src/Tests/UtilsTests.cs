using System.Text.Json;
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
}