using Microsoft.AspNetCore.HttpOverrides;

namespace HackerNewsCommentsFeed.Middleware;

public static class Middleware
{
    public static IApplicationBuilder AddMiddleware(this IApplicationBuilder app)
    {
        app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            })
            .UseCors()
            .UseAuthentication()
            .UseAuthorization()
            .UseMiddleware<HttpRequestsInterceptor>()
            .UseSwagger()
            .UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            })
            .UseMiddleware<CustomExceptionHandler>(); // TODO: check where it should be in the pipeline

        return app;
    }
}