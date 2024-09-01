using Application.Sort;
using FluentAssertions;
using Xunit;

namespace Tests.Unit;

public class SortingParametersParserTests
{
    [Fact]
    public void Sorting_params_title_asc_and_score_desc_should_be_parsed_correctly()
    {
        // Arrange
        var query = "title asc, score desc";
        var expectedResult = new List<SortingParameters>()
        {
            new(SortOrder.Asc, SortField.Title),
            new(SortOrder.Desc, SortField.Score),
        };

        // Act
        var sortingParams = SortingParametersParser.Parse(query);

        // Assert
        sortingParams.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Sorting_params_score_desc_and_id_asc_should_be_parsed_correctly()
    {
        // Arrange
        var query = "score desc, id asc";
        var expectedResult = new List<SortingParameters>()
        {
            new(SortOrder.Desc, SortField.Score),
            new(SortOrder.Asc, SortField.Id),
        };

        // Act
        var sortingParams = SortingParametersParser.Parse(query);

        // Assert
        sortingParams.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Sorting_param_score_should_be_parsed_correctly()
    {
        // Arrange
        var query = "score";
        var expectedResult = new List<SortingParameters>()
        {
            new(SortOrder.Desc, SortField.Score),
        };

        // Act
        var sortingParams = SortingParametersParser.Parse(query);

        // Assert
        sortingParams.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Sorting_params_score_and_title_should_be_parsed_correctly()
    {
        // Arrange
        var query = "score, title";
        var expectedResult = new List<SortingParameters>()
        {
            new(SortOrder.Desc, SortField.Score),
            new(SortOrder.Desc, SortField.Title),
        };

        // Act
        var sortingParams = SortingParametersParser.Parse(query);

        // Assert
        sortingParams.Should().BeEquivalentTo(expectedResult);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Empty_sorting_params_should_be_discarded(string? query)
    {
        // Arrange
        var expectedResult = new List<SortingParameters>()
        {
            new(SortOrder.Desc, SortField.Score)
        };

        // Act
        var sortingParamsAct = SortingParametersParser.Parse(query);

        // Assert
        sortingParamsAct.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Incorrectly_formatted_sorting_params_should_be_discarded()
    {
        // Arrange
        var sortingParameters = "author,title ascending";
        var expectedResult = new List<SortingParameters>()
        {
            new(SortOrder.Desc, SortField.Score),
            new(SortOrder.Desc, SortField.Title),
        };

        // Act
        var sortingParamsAct = SortingParametersParser.Parse(sortingParameters);

        // Assert
        sortingParamsAct.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Incorrectly_formatted_sorting_param_should_be_discarded()
    {
        // Arrange
        var sortingParameters = "author";
        var expectedResult = new List<SortingParameters>()
        {
            new(SortOrder.Desc, SortField.Score),
        };

        // Act
        var sortingParamsAct = SortingParametersParser.Parse(sortingParameters);

        // Assert
        sortingParamsAct.Should().BeEquivalentTo(expectedResult);
    }
}