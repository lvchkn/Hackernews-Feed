using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Db;

namespace Tests.Integration;

[Collection("Test server collection")]
public class InterestsControllerTests
{
    private readonly TestServerFixture<Program> _webAppFactory;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    private record CreateInterestRequest(string Text, int? Id);

    public InterestsControllerTests(TestServerFixture<Program> webAppFactory)
    {
        _webAppFactory = webAppFactory;
    }

    private static async Task<JsonElement> ReadProblemDetails(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        using var doc = JsonDocument.Parse(json);
        return doc.RootElement.Clone();
    }

    [Fact]
    public async Task Add_New_Interest_Then_Get_It_By_Id()
    {
        // Arrange
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
        var postedInterestId = JsonSerializer.Deserialize<int>(postResponseJson, _jsonSerializerOptions);

        // Act 2 - get newly added interest by id
        var getByIdResponse = await client.GetAsync($"/api/interests/{postedInterestId}", TestContext.Current.CancellationToken);
    
        var getByIdResponseJson = await getByIdResponse.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        var actualInterest = JsonSerializer.Deserialize<CreateInterestRequest>(getByIdResponseJson, _jsonSerializerOptions);
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

        var returnedUpdatedInterest = JsonSerializer.Deserialize<CreateInterestRequest>(getByIdUpdatedResponseJson, _jsonSerializerOptions);

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

    [Fact]
    public async Task GetInterest_NotFound_Returns404()
    {
        // Arrange
        var client = _webAppFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        
        // Act
        var response = await client.GetAsync($"/api/interests/999999", TestContext.Current.CancellationToken);
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        var problem = await ReadProblemDetails(response);
        Assert.Equal((int)response.StatusCode, problem.GetProperty("status").GetInt32());
        Assert.True(problem.GetProperty("title").GetString() is not null);
        Assert.True(problem.GetProperty("detail").GetString() is not null);
    }

    [Fact]
    public async Task AddInterest_Duplicate_ReturnsBadRequest()
    {
        // Arrange
        var client = _webAppFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        var interestDto = new CreateInterestRequest("Duplicate Interest", -1);
        var serializedInterest = JsonSerializer.Serialize(interestDto, _jsonSerializerOptions);
        var content = new StringContent(serializedInterest);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        // Act
        var firstInterest = await client.PostAsync("/api/interests", content, TestContext.Current.CancellationToken);
        if (!firstInterest.IsSuccessStatusCode)
        {
            var err = await firstInterest.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            throw new Exception($"POST /api/interests failed: {firstInterest.StatusCode} - {err}");
        }

        // Act 2 - try to add again
        var secondContent = new StringContent(serializedInterest);
        secondContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var secondInterest = await client.PostAsync("/api/interests", secondContent, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, secondInterest.StatusCode);

        Assert.Equal("application/problem+json", secondInterest.Content.Headers.ContentType?.MediaType);
        var pd = await ReadProblemDetails(secondInterest);
        Assert.Equal((int)secondInterest.StatusCode, pd.GetProperty("status").GetInt32());
        Assert.True(pd.GetProperty("title").GetString() is not null);
        Assert.Contains("Duplicate", pd.GetProperty("detail").GetString());
    }

    [Fact]
    public async Task UpdateInterest_ValidationErrors_ReturnsBadRequest()
    {
        // Arrange
        var client = _webAppFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        using var scope = _webAppFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.SeedUsers();

        var interestDto = new CreateInterestRequest("ToBeUpdated", -1);
        var serializedInterest = JsonSerializer.Serialize(interestDto, _jsonSerializerOptions);
        var content = new StringContent(serializedInterest);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        // Act
        var postResponse = await client.PostAsync("/api/interests", content, TestContext.Current.CancellationToken);
        postResponse.EnsureSuccessStatusCode();
        var idJson = await postResponse.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        var createdId = JsonSerializer.Deserialize<int>(idJson, _jsonSerializerOptions);

        var badContent = new StringContent("{ this is : not json }");
        badContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var putResponse = await client.PutAsync($"/api/interests/{createdId}", badContent, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, putResponse.StatusCode);

        Assert.Equal("application/problem+json", putResponse.Content.Headers.ContentType?.MediaType);
        var pd = await ReadProblemDetails(putResponse);
        Assert.Equal((int)putResponse.StatusCode, pd.GetProperty("status").GetInt32());
        Assert.True(pd.GetProperty("title").GetString() is not null);
    }

    [Fact]
    public async Task Authentication_Authorization_Enforced()
    {
        // Arrange
        var unauthenticatedClient = _webAppFactory.CreateClient();
        var authClient = _webAppFactory.CreateClient();
        authClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        // Act 1 - unauthorized request should get 401 for protected endpoint
        var unauthenticatedResponse = await unauthenticatedClient.GetAsync("/api/interests", TestContext.Current.CancellationToken);

        // Act 2 - authenticated request
        var authResp = await authClient.GetAsync("/api/interests", TestContext.Current.CancellationToken);

        // Assert        
        Assert.Equal(HttpStatusCode.Unauthorized, unauthenticatedResponse.StatusCode);
        authResp.EnsureSuccessStatusCode();
    }
}