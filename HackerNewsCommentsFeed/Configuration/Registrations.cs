using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace HackerNewsCommentsFeed.Configuration;

public static class Registrations
{
    private static IConfiguration? _configuration;
    public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        _configuration = configuration;

        services
            .AddApplication(_configuration)
            .AddInfrastructure(_configuration)
            .AddGithubAuth()
            .AddAuthorization()
            .AddCorsPolicies()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();
    }
    
    private static IServiceCollection AddGithubAuth(this IServiceCollection services)
    {
        var clientId = _configuration?.GetValue<string>("GithubAuth:ClientId");
        var clientSecret = _configuration?.GetValue<string>("GithubAuth:ClientSecret");
        var callbackPath = _configuration?.GetValue<string>("GithubAuth:CallbackPath");
        var authorizationEndpoint = _configuration?.GetValue<string>("GithubAuth:AuthorizationEndpoint");
        var tokenEndpoint = _configuration?.GetValue<string>("GithubAuth:TokenEndpoint");
        var userInformationEndpoint = _configuration?.GetValue<string>("GithubAuth:UserInformationEndpoint");
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = "Github";
        })
        .AddCookie()
        .AddOAuth("Github", config =>
        {
            config.ClientId = clientId ?? string.Empty;
            config.ClientSecret = clientSecret ?? string.Empty;
            config.CallbackPath = new PathString(callbackPath);
            config.AuthorizationEndpoint = authorizationEndpoint ?? string.Empty;
            config.TokenEndpoint = tokenEndpoint ?? string.Empty;
            config.UserInformationEndpoint = userInformationEndpoint ?? string.Empty;
            config.SaveTokens = true;
            config.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            config.ClaimActions.MapJsonKey(ClaimTypes.Name, "login");
            config.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            config.ClaimActions.MapJsonKey("urn:github:name", "name");
            config.ClaimActions.MapJsonKey("urn:github:url", "url");
            config.Scope.Add("user:email");
            config.Events = new OAuthEvents
            {
                OnCreatingTicket = async context =>
                {
                    using var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                    using var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                    response.EnsureSuccessStatusCode();
                    
                    using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(context.HttpContext.RequestAborted));
                    var root = payload.RootElement;
                    context.RunClaimActions(root);
                },

                OnRedirectToAuthorizationEndpoint = context => 
                {
                    if (context.Request.Path.ToString().Contains("api"))
                    {
                        context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                    }
                    else
                    {
                        context.Response.Redirect(context.RedirectUri);
                    }

                    return Task.CompletedTask;
                },
            };
        });

        return services;
    }

    private static IServiceCollection AddCorsPolicies(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy  => 
            {
                policy.WithOrigins("http://localhost:8080")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }
}