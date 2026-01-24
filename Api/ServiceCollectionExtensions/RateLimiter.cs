using System.Threading.RateLimiting;

namespace Api.ServiceCollectionExtensions;

public static class RateLimiter
{
    public static IServiceCollection AddRateLimiter(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            {
                var isAuthenticated = httpContext.User.Identity?.IsAuthenticated ?? false;
                string partitionKey = isAuthenticated
                    ? $"auth:{httpContext.User.Identity?.Name ?? "unknown"}"
                    : $"ip:{httpContext.Connection.RemoteIpAddress?.ToString() ?? "unauthorized"}";

                return RateLimitPartition.GetTokenBucketLimiter(
                    partitionKey,
                    _ => new TokenBucketRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        ReplenishmentPeriod = TimeSpan.FromSeconds(10),
                        TokensPerPeriod = isAuthenticated ? 50 : 20,
                        TokenLimit = isAuthenticated ? 200 : 60,
                        QueueLimit = 0,
                    });
            });

            options.OnRejected = async (context, cancellationToken) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.HttpContext.Response.ContentType = "application/problem+json";

                var problem = new Microsoft.AspNetCore.Mvc.ProblemDetails
                {
                    Title = "Too Many Requests",
                    Status = StatusCodes.Status429TooManyRequests,
                    Detail = "Rate limit exceeded. Try again later.",
                    Instance = context.HttpContext.Request.Path
                };

                await context.HttpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
            };
        });

        return services;
    }
}