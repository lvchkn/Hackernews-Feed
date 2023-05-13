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
    private record CreateInterestRequest(string Text, int? Id);

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

        var interest = new CreateInterestRequest("Cloud Native Technologies", null);
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
        var postedInterestId = JsonSerializer.Deserialize<int>(postResponseJson, jsonSerializerOptions);

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

        // Act 3 - update interest
        var updatedInterest = actualPostedInterest with { Text = "Updated Cloud Native Technologies" };
        var updatedInterestStringContent = new StringContent(serializedInterest);
        updatedInterestStringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        
        var putResponse = await client.PutAsync($"/api/interests/{interest.Text}", updatedInterestStringContent);
        
        // Assert 3 - update interest
        putResponse.EnsureSuccessStatusCode();
        putResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Act 4 - get updated interest by name
        var getByIdUpdatedResponse = await client.GetAsync($"/api/interests/{updatedInterest.Text}");
    
        var getByIdUpdatedResponseJson = await getByIdResponse.Content.ReadAsStringAsync();

        var returnedUpdatedInterest = JsonSerializer.Deserialize<CreateInterestRequest>(getByIdResponseJson, jsonSerializerOptions);

        // Assert 4 - the interest returned by id is equivalent to the one we just updated
        returnedUpdatedInterest?.Should().BeEquivalentTo(updatedInterest);

        // Act 5 - delete interest
        var deleteResponse = await client.DeleteAsync($"/api/interests/{returnedUpdatedInterest?.Id}");

        // Assert 5 - delete interest
        deleteResponse.EnsureSuccessStatusCode();
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Act 6 - try to get the deleted interest
        var getByIdDeletedResponse = await client.GetAsync($"/api/interests/{returnedUpdatedInterest?.Id}");
    
        var getByIdDeletedResponseJson = await getByIdResponse.Content.ReadAsStringAsync();

        // Assert 6 - delete interest
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}