using Application.Services.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Shared.Utils;

namespace HackerNewsCommentsFeed.Controllers;

public static class AuthController
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/login", async (
            HttpContext httpContext,
            [FromQuery] string? returnUrl,
            [FromServices] IUsersService usersService) =>
        {
            await httpContext.ChallengeAsync(new AuthenticationProperties
            {
                RedirectUri = returnUrl ?? "/"
            });

            var userInfo = GetCurrentUserInfo(httpContext);

            return Results.Ok(userInfo);
            
        }).WithTags(EndpointGroupTags.Authentication);

        app.MapGet("/userinfo", (HttpContext httpContext) =>
        {
            var userInfo = GetCurrentUserInfo(httpContext);

            return Results.Ok(userInfo);
            
        }).WithTags(EndpointGroupTags.Authentication);

        return app;
    }

    private record UserInfo(string Name, string AuthenticationType, bool IsAuthenticated);
    private static UserInfo GetCurrentUserInfo(HttpContext httpContext)
    {
        var userInfo = new UserInfo(string.Empty, string.Empty, false);

        if (httpContext.User.Identity is { IsAuthenticated: true } authenticatedUser)
        {
            userInfo = userInfo with
            {
                Name = authenticatedUser?.Name ?? string.Empty,
                AuthenticationType = authenticatedUser?.AuthenticationType ?? string.Empty,
                IsAuthenticated = authenticatedUser?.IsAuthenticated ?? false,
            };
        }

        return userInfo;
    }
}