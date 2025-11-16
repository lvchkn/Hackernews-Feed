using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Shared.Exceptions;

namespace Api.ServiceCollectionExtensions;

public static class GithubAuth
{
    private record GithubAuthSettings
    {
        public required string ClientId { get; init; }
        public required string ClientSecret { get; init; }
        public required string CallbackPath { get; init; }
        public required string AuthorizationEndpoint { get; init; }
        public required string TokenEndpoint { get; init; }
        public required string UserInformationEndpoint { get; init; }
    }

    public static IServiceCollection AddGithubAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = "Github";
        })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => {
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        })
        .AddOAuth("Github", config =>
        {
            var githubAuth = configuration.GetSection("GithubAuth").Get<GithubAuthSettings>() 
                             ?? throw new ConfigurationException("GithubAuth section is missing");

            config.ClientId = githubAuth.ClientId;
            config.ClientSecret = githubAuth.ClientSecret;
            config.CallbackPath = new PathString(githubAuth.CallbackPath);
            config.AuthorizationEndpoint = githubAuth.AuthorizationEndpoint;
            config.TokenEndpoint = githubAuth.TokenEndpoint;
            config.UserInformationEndpoint = githubAuth.UserInformationEndpoint;
            config.SaveTokens = false;
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
                    context.RunClaimActions(payload.RootElement);
                },

                OnRedirectToAuthorizationEndpoint = context => 
                {
                    if (context.Request.Path.StartsWithSegments("api"))
                    {
                        context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                    }
                    else
                    {
                        context.Response.Redirect(context.RedirectUri);
                    }

                    return Task.CompletedTask;
                },
                
                // OnRemoteFailure = context =>
                // {
                //     logger.LogError(context.Failure?.ToString() ?? "Error occurred during authentication");
                //     return Task.CompletedTask;
                // }
            };
        });

        return services;
    }
}