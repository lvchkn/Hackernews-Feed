namespace HackerNewsCommentsFeed.Configuration;

public static class Middleware
{
    private static IConfiguration? _configuration;

    public static IApplicationBuilder UseMiddleware(this IApplicationBuilder app, IConfiguration configuration)
    {
        _configuration = configuration;

        app.UseCors()
            .UseAuthentication()
            .UseAuthorization()
            .UseMiddleware<HttpRequestsInterceptor>()
            .UseSwagger()
            .UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            })
            .UseMiddleware<CustomExceptionHandler>();

        return app;
    }
}