using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Application.Users;
using Microsoft.AspNetCore.Authentication;
using Shared.Utils;

namespace HackerNewsCommentsFeed.Configuration;

public class HttpRequestsInterceptor
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public HttpRequestsInterceptor(
        IConfiguration configuration,
        RequestDelegate next)
    {
        _configuration = configuration;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, IUsersService usersService)
    {
        var userInformationEndpoint = _configuration?.GetValue<string>("GithubAuth:UserInformationEndpoint");
        var appName = _configuration?.GetValue<string>("GithubAuth:AppName");

        var accessToken = await httpContext.GetTokenAsync("access_token");

        if (httpContext.User.Identity is { IsAuthenticated: true })
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, userInformationEndpoint);
            using var client = new HttpClient();

            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(appName ?? string.Empty, "1.0"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", accessToken);

            using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var root = payload.RootElement;
                
            var keyExists = root.TryGetProperty("login", out var res);
            var name = string.Empty;

            if (keyExists)
            {
                name = res.GetString();
            }
            
            const string email = "example@example.com";
            //var email = httpContext.User.Claims.FirstOrDefault(c => c.Type == "emails");
            var claim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            int.TryParse(claim?.Value ?? string.Empty, out var userId);

            try
            {
                await usersService.UpdateLastActiveAsync(userId);
            }
            catch (NotFoundException)
            {
                var newUser = new UserDto
                {
                    Id = userId,
                    Name = name ?? string.Empty,
                    Email = email,
                    LastActive = DateTime.UtcNow,
                };

                await usersService.AddAsync(newUser);
            }
        }

        await _next(httpContext);
    }
}