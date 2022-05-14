using System.Net.Http.Headers;
using System.Text.Json;
using HackerNewsCommentsFeed.Domain;
using HackerNewsCommentsFeed.Repositories;
using Hangfire;
using Microsoft.AspNetCore.Authentication;

namespace HackerNewsCommentsFeed.DI;

public static class Middleware
{
    private static IConfiguration? _configuration;
    public static IApplicationBuilder UseMiddleware(this IApplicationBuilder app, IConfiguration configuration)
    {
        _configuration = configuration;
        
        app.UseCors()
            .UseAuthentication()
            .UseAuthorization()
            .UseHttpRequestsInterceptor()
            .UseHangfireDashboard()
            .UseSwagger()
            .UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });

        return app;
    }

    private static IApplicationBuilder UseHttpRequestsInterceptor(this IApplicationBuilder app)
    {
        var userInformationEndpoint = _configuration.GetValue<string>("GithubAuth:UserInformationEndpoint");
        var appName = _configuration.GetValue<string>("GithubAuth:AppName");

        app.Use(async (httpContext, next) =>
        {
            var accessToken = await httpContext.GetTokenAsync("access_token");
                
            if (httpContext.User.Identity is {IsAuthenticated: true})
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, userInformationEndpoint);
                using var client = new HttpClient();
                
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(appName, "1.0"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", accessToken);
                
                using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                var root = payload.RootElement;
                
                var usersRepository = app.ApplicationServices.GetService<IUsersRepository>();
                const string email = "example@example.com";
                //var email = httpContext.User.Claims.FirstOrDefault(c => c.Type == "emails");
                usersRepository?.AddUserAsync(new User {Name = root.GetProperty("login").GetString(), Email = email});
                usersRepository?.UpdateLastActiveAsync(email);
            }
            
            await next(httpContext);
        });

        return app;
    }
}