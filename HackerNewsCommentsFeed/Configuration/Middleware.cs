using System.Net.Http.Headers;
using System.Text.Json;
using Application.Contracts;
using Application.Services.Users;
using Microsoft.AspNetCore.Authentication;
using Shared.Utils;

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
            .UseHttpRequestsInterceptor()
            .UseSwagger()
            .UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            })
            .UseMiddleware<CustomExceptionHandler>();

        return app;
    }

    private static IApplicationBuilder UseHttpRequestsInterceptor(this IApplicationBuilder app)
    {
        var userInformationEndpoint = _configuration.GetValue<string>("GithubAuth:UserInformationEndpoint");
        var appName = _configuration.GetValue<string>("GithubAuth:AppName");

        app.Use(async (httpContext, next) =>
        {
            var accessToken = await httpContext.GetTokenAsync("access_token");

            if (httpContext.User.Identity is { IsAuthenticated: true })
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, userInformationEndpoint);
                using var client = new HttpClient();

                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(appName, "1.0"));
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

                using var scope = app.ApplicationServices.CreateScope();
                var usersService = scope.ServiceProvider.GetService<IUsersService>();
                const string email = "example@example.com";
                //var email = httpContext.User.Claims.FirstOrDefault(c => c.Type == "emails");

                try
                {
                    var updateResult = await usersService!.UpdateLastActiveAsync(email);
                }
                catch (NotFoundException)
                {
                    var newUser = new UserDto
                    {
                        Name = name ?? "DEFAULT_USER",
                        Email = email,
                    };

                    var createResult = await usersService!.AddAsync(newUser);
                }
            }

            await next(httpContext);
        });

        return app;
    }
}