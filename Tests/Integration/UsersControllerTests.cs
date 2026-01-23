using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Application.Interests;
using Domain.Entities;
using Infrastructure.Db;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Tests.Integration;

[Collection("Test server collection")]
public class UsersControllerTests
{
    private readonly TestServerFixture<Program> _webAppFactory;

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public UsersControllerTests(TestServerFixture<Program> webAppFactory)
    {
        _webAppFactory = webAppFactory;
    }

    [Fact]
    public async Task GetUserInterests_ReturnsSeededInterests()
    {
        // Arrange
        var client = _webAppFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        using var scope = _webAppFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.SeedUsers();

        var user = dbContext.Users.OrderBy(u => u.Id).First();

        // Act
        var response = await client.GetAsync($"/api/user/{user.Id}/interests", TestContext.Current.CancellationToken);
        var responseJson = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        var interests = JsonSerializer.Deserialize<List<InterestDto>>(responseJson, _jsonSerializerOptions);

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(interests);
        Assert.Equal(user.Interests.Count, interests.Count);
    }

    [Fact]
    public async Task AddInterestToUser_WhenInterestExists_ReturnsCreatedAndIsAdded()
    {
        // Arrange
        var client = _webAppFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        using var scope = _webAppFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.SeedUsers();

        var user = dbContext.Users.OrderBy(u => u.Id).First();

        // create a standalone interest to attach
        var interest = new Interest { Text = "Kubernetes" };
        dbContext.Interests.Add(interest);
        dbContext.SaveChanges();

        var interestDto = new InterestDto { Id = interest.Id, Text = interest.Text };
        var serialized = JsonSerializer.Serialize(interestDto, _jsonSerializerOptions);
        var content = new StringContent(serialized);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        // Act
        var postResponse = await client.PostAsync($"/api/user/{user.Id}/interests", content, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

        // Verify interest is now associated with user
        var getResponse = await client.GetAsync($"/api/user/{user.Id}/interests", TestContext.Current.CancellationToken);
        var getResponseJson = await getResponse.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        var interests = JsonSerializer.Deserialize<List<InterestDto>>(getResponseJson, _jsonSerializerOptions);

        Assert.Contains(interests!, i => i.Id == interest.Id && i.Text == interest.Text);
    }

    [Fact]
    public async Task AddInterestToUser_WhenInterestDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var client = _webAppFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        using var scope = _webAppFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.SeedUsers();

        var user = dbContext.Users.OrderBy(u => u.Id).First();

        var interestDto = new InterestDto { Id = 999_999, Text = "NonExisting" };
        var serialized = JsonSerializer.Serialize(interestDto, _jsonSerializerOptions);
        var content = new StringContent(serialized);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        // Act
        var postResponse = await client.PostAsync($"/api/user/{user.Id}/interests", content, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, postResponse.StatusCode);

        Assert.Equal("application/problem+json", postResponse.Content.Headers.ContentType?.MediaType);
        var json = await postResponse.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;
        Assert.Equal((int)postResponse.StatusCode, root.GetProperty("status").GetInt32());
        Assert.True(root.GetProperty("title").GetString() is not null);
        Assert.True(root.GetProperty("detail").GetString() is not null);
    }

    [Fact]
    public async Task DeleteInterest_RemovesInterestFromUser()
    {
        // Arrange
        var client = _webAppFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        using var scope = _webAppFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.SeedUsers();

        var user = dbContext.Users.OrderBy(u => u.Id).First();
        var interest = user.Interests.First();

        // Act
        var deleteResponse = await client.DeleteAsync($"/api/user/{user.Id}/interests/{interest.Id}", TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        // Verify it's removed
        var getResponse = await client.GetAsync($"/api/user/{user.Id}/interests", TestContext.Current.CancellationToken);
        var getResponseJson = await getResponse.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        var interests = JsonSerializer.Deserialize<List<InterestDto>>(getResponseJson, _jsonSerializerOptions);

        Assert.DoesNotContain(interests!, i => i.Id == interest.Id);
    }
}
