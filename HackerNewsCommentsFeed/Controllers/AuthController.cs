using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Shared.Utils;

namespace HackerNewsCommentsFeed.Controllers;

public static class AuthController
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/login", ([FromQuery] string? returnUrl) =>
        {
            return Results.Challenge(new AuthenticationProperties
            {
                RedirectUri = returnUrl ?? "/",
            }, new [] { "Github" });
            
        }).WithTags(EndpointGroupTags.Authentication);

        app.MapGet("/logout", ([FromQuery] string? returnUrl) =>
        {
            return Results.SignOut(new AuthenticationProperties
            {
                RedirectUri = returnUrl ?? "/",
            });
            
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