using Microsoft.AspNetCore.HttpOverrides;

namespace Api.Middleware;

public static class Middleware
{
    public static IApplicationBuilder AddMiddleware(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            })
            .UseMiddleware<CustomExceptionHandler>()
            .UseCors()
            .UseAuthentication()
            .UseAuthorization()
            .UseMiddleware<HttpRequestsInterceptor>();

        if (environment.IsDevelopment())
        {
            app.UseSwagger()
                .UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
        }
        
        return app;
    }
}