using System.Net.Http.Headers;

namespace Tests.Integration;

public static class TestClientExtensions
{
    public static HttpClient WithTestAuth(this HttpClient client, bool isAdmin = false)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        client.DefaultRequestHeaders.Add("X-Test-Admin", isAdmin ? "true" : "false");
        
        return client;
    }
}
