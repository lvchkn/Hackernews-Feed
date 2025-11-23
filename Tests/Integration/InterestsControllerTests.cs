using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Xunit;
using Api;

namespace Tests.Integration;

[Collection("Test server collection")]
public class InterestsControllerTests
{
    private readonly TestServerFixture<Program> _webAppFactory;
    private record CreateInterestRequest(string Text, int? Id);

    public InterestsControllerTests(TestServerFixture<Program> webAppFactory)
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

        var interest = new CreateInterestRequest("Cloud Native Technologies", -1);
        var serializedInterest = JsonSerializer.Serialize(interest);
        
        var interestString = new StringContent(serializedInterest);
        interestString.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var client = _webAppFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        
        // Act 1 - add new interest
        var postResponse = await client.PostAsync("/api/interests", interestString, TestContext.Current.CancellationToken);

        // Assert 1 - new interest is added
        postResponse.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, postResponse.StatusCode);
        
        var postResponseJson = await postResponse.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        var postedInterestId = JsonSerializer.Deserialize<int>(postResponseJson, jsonSerializerOptions);

        // Act 2 - get newly added interest by id
        var getByIdResponse = await client.GetAsync($"/api/interests/{postedInterestId}", TestContext.Current.CancellationToken);
    
        var getByIdResponseJson = await getByIdResponse.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        var actualInterest = JsonSerializer.Deserialize<CreateInterestRequest>(getByIdResponseJson, jsonSerializerOptions);
        var expectedInterest = new
        {
            Id = postedInterestId,
            Text = interest.Text,
        };

        // Assert 2 - the interest returned by id is equivalent to the one we just added
        Assert.Equivalent(expectedInterest, actualInterest);

        // Act 3 - update interest
        var updatedInterest = expectedInterest with { Text = "Updated Cloud Native Technologies" };
        var serializedUpdatedInterest = JsonSerializer.Serialize(updatedInterest);

        var updatedInterestStringContent = new StringContent(serializedUpdatedInterest);
        updatedInterestStringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        
        var putResponse = await client.PutAsync($"/api/interests/{postedInterestId}", updatedInterestStringContent, TestContext.Current.CancellationToken);
        
        // Assert 3 - update interest
        putResponse.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, putResponse.StatusCode);

        // Act 4 - get updated interest by name
        var getByIdUpdatedResponse = await client.GetAsync($"/api/interests/{updatedInterest.Id}", TestContext.Current.CancellationToken);
    
        var getByIdUpdatedResponseJson = await getByIdUpdatedResponse.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        var returnedUpdatedInterest = JsonSerializer.Deserialize<CreateInterestRequest>(getByIdUpdatedResponseJson, jsonSerializerOptions);

        // Assert 4 - the interest returned by id is equivalent to the one we just updated
        Assert.Equivalent(updatedInterest, returnedUpdatedInterest);

        // Act 5 - delete interest
        var deleteResponse = await client.DeleteAsync($"/api/interests/{returnedUpdatedInterest?.Id}", TestContext.Current.CancellationToken);

        // Assert 5 - delete interest
        deleteResponse.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        // Act 6 - try to get the deleted interest
        var getByIdDeletedResponse = await client.GetAsync($"/api/interests/{returnedUpdatedInterest?.Id}", TestContext.Current.CancellationToken);
        
        // Assert 6 - delete interest
        Assert.Equal(HttpStatusCode.NotFound, getByIdDeletedResponse.StatusCode);
    }
}