using Microsoft.AspNetCore.Authentication;

namespace HackerNewsCommentsFeed.Controllers;

public static class AuthController
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/login", (string? returnUrl) =>
        {
            return Results.Challenge(new AuthenticationProperties
            {
                RedirectUri = returnUrl ?? "/"
            });
        });

        app.MapGet("/userinfo", (HttpContext httpContext) =>
        {
            var userInfo = new
            {
                Name = "",
                AuthenticationType = "",
                IsAuthenticated = false,
            };

            if (httpContext.User.Identity is {IsAuthenticated: true} authenticatedUser)
            {
                userInfo = userInfo with
                {
                    Name = authenticatedUser.Name,
                    AuthenticationType = authenticatedUser.AuthenticationType,
                    IsAuthenticated = authenticatedUser.IsAuthenticated,
                };
            }

            return Results.Ok(userInfo);
        });
        
        return app;
    }
}