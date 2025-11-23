using System.Security.Claims;
using Application.Users;
using Shared.Exceptions;

namespace Api.Middleware;

public class HttpRequestsInterceptor
{
    private readonly RequestDelegate _next;
    private readonly ILogger<HttpRequestsInterceptor> _logger;

    public HttpRequestsInterceptor(RequestDelegate next, ILogger<HttpRequestsInterceptor> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext, IUsersService usersService)
    {
        if (httpContext.User.Identity is { IsAuthenticated: false })
        {
            await _next(httpContext);
            return;
        }

        var userIdString = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        var userName = httpContext.User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        //var userEmail = httpContext.User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
        
        var userId = 0;
        try
        {
            userId = int.Parse(userIdString);
        }
        catch (Exception)
        {
            _logger.LogWarning("User's id {UserId} is not a valid integer.", userIdString);
        }        
        
        try
        {
            var user = await usersService.GetByIdAsync(userId);
            
            if (userId != 0 && (DateTime.UtcNow - user.LastActive).TotalMinutes > 30)
            {
                await usersService.UpdateLastActiveAsync(userId);
            }
        }
        catch (UserNotFoundException)
        {
            var newUser = new UserDto
            {
                Id = userId,
                Name = userName,
                Email = "example@example.com",
                LastActive = DateTime.UtcNow,
            };

            await usersService.AddAsync(newUser);
        }

        await _next(httpContext);
    }
}