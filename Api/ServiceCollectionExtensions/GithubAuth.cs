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
    private const string ClientId = nameof(ClientId);
    private const string ClientSecret = nameof(ClientSecret);
    private const string CallbackPath = nameof(CallbackPath);
    private const string AuthorizationEndpoint = nameof(AuthorizationEndpoint);
    private const string TokenEndpoint = nameof(TokenEndpoint);
    private const string UserInformationEndpoint = nameof(UserInformationEndpoint);

    public static IServiceCollection AddGithubAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var githubAuth = GetAppSettings(configuration);

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
            config.ClientId = githubAuth[ClientId]!;
            config.ClientSecret = githubAuth[ClientSecret]!;
            config.CallbackPath = new PathString(githubAuth[CallbackPath]);
            config.AuthorizationEndpoint = githubAuth[AuthorizationEndpoint]!;
            config.TokenEndpoint = githubAuth[TokenEndpoint]!;
            config.UserInformationEndpoint = githubAuth[UserInformationEndpoint]!;
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

    private static Dictionary<string, string?> GetAppSettings(IConfiguration configuration)
    {
        var githubAuthSettings = new Dictionary<string, string?>
        {
            [ClientId] = configuration.GetValue<string>("GithubAuth:ClientId"),
            [ClientSecret] = configuration.GetValue<string>("GithubAuth:ClientSecret"),
            [CallbackPath] = configuration.GetValue<string>("GithubAuth:CallbackPath"),
            [AuthorizationEndpoint] = configuration.GetValue<string>("GithubAuth:AuthorizationEndpoint"),
            [TokenEndpoint] = configuration.GetValue<string>("GithubAuth:TokenEndpoint"),
            [UserInformationEndpoint] = configuration.GetValue<string>("GithubAuth:UserInformationEndpoint")
        };

        foreach (var (key, value) in githubAuthSettings)
        {
            if (value is null)
            {
                throw new ConfigurationException($"{key} app setting is missing!");
            }
        }

        return githubAuthSettings;
    }
}