using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace Tests.Integration;

public class InterestsControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _webAppFactory;
    private record UserInfoResponse(bool IsAuthenticated, string AuthenticationType, string Name);
    private record CreateInterestRequest(string Id, string Text);

    public InterestsControllerTests(CustomWebApplicationFactory<Program> webAppFactory)
    {
        _webAppFactory = webAppFactory;   
    }

    [Fact]
    public async Task Add_New_Interest_Then_Get_It_By_Id()
    {
        // Arrange
        var jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var interest = new CreateInterestRequest(string.Empty, "Cloud Native Technologies");
        var serializedInterest = JsonSerializer.Serialize(interest);
        
        var interestString = new StringContent(serializedInterest);
        interestString.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var client = _webAppFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        
        // Act 1 - add new interest
        var postResponse = await client.PostAsync("/api/interests", interestString);

        // Assert 1 - new interest is added
        postResponse.EnsureSuccessStatusCode();
        postResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var postResponseJson = await postResponse.Content.ReadAsStringAsync();
        var postedInterestId = JsonSerializer.Deserialize<string>(postResponseJson, jsonSerializerOptions);

        // Act 2 - get newly added interest by id
        var getByIdResponse = await client.GetAsync($"/api/interests/{interest.Text}");
    
        var getByIdResponseJson = await getByIdResponse.Content.ReadAsStringAsync();

        var returnedinterest = JsonSerializer.Deserialize<CreateInterestRequest>(getByIdResponseJson, jsonSerializerOptions);
        var actualPostedInterest = new
        {
            Id = postedInterestId,
            Text = interest.Text,
        };

        // Assert 2 - the interest returned by id is equivalent to the one we just added
        returnedinterest?.Should().BeEquivalentTo(actualPostedInterest);
    }
}