using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Shared.Exceptions;

namespace HackerNewsCommentsFeed.ServiceCollectionExtensions;

public static class GithubAuth
{
    private static bool SomeValueIsNull(params string?[] values)
    {
        return values.Any(v => v is null);
    }
    
    public static IServiceCollection AddGithubAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var clientId = configuration.GetValue<string>("GithubAuth:ClientId");
        var clientSecret = configuration.GetValue<string>("GithubAuth:ClientSecret");
        var callbackPath = configuration.GetValue<string>("GithubAuth:CallbackPath");
        var authorizationEndpoint = configuration.GetValue<string>("GithubAuth:AuthorizationEndpoint");
        var tokenEndpoint = configuration.GetValue<string>("GithubAuth:TokenEndpoint");
        var userInformationEndpoint = configuration.GetValue<string>("GithubAuth:UserInformationEndpoint");

        if (SomeValueIsNull(clientId,
                clientSecret,
                callbackPath,
                authorizationEndpoint,
                tokenEndpoint,
                userInformationEndpoint))
        {
            throw new ConfigurationException("Some app setting is missing!");
        }
        
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
            config.ClientId = clientId!;
            config.ClientSecret = clientSecret!;
            config.CallbackPath = new PathString(callbackPath);
            config.AuthorizationEndpoint = authorizationEndpoint!;
            config.TokenEndpoint = tokenEndpoint!;
            config.UserInformationEndpoint = userInformationEndpoint!;
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
}