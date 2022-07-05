using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
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
            .AddGithubAuth()
            .AddAuthorization()
            .AddCorsPolicies()
            .AddHangfireWithMongoStorage()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();
    }
    
    private static IServiceCollection AddGithubAuth(this IServiceCollection services)
    {
        var clientId = _configuration.GetValue<string>("GithubAuth:ClientId");
        var clientSecret = _configuration.GetValue<string>("GithubAuth:ClientSecret");
        var callbackPath = _configuration.GetValue<string>("GithubAuth:CallbackPath");
        var authorizationEndpoint = _configuration.GetValue<string>("GithubAuth:AuthorizationEndpoint");
        var tokenEndpoint = _configuration.GetValue<string>("GithubAuth:TokenEndpoint");
        var userInformationEndpoint = _configuration.GetValue<string>("GithubAuth:UserInformationEndpoint");
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = "Github";
        })
        .AddCookie()
        .AddOAuth("Github", config =>
        {
            config.ClientId = clientId;
            config.ClientSecret = clientSecret;
            config.CallbackPath = new PathString(callbackPath);
            config.AuthorizationEndpoint = authorizationEndpoint;
            config.TokenEndpoint = tokenEndpoint;
            config.UserInformationEndpoint = userInformationEndpoint;
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
                policy.WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }

    private static IServiceCollection AddHangfireWithMongoStorage(this IServiceCollection serviceCollection)
    {
        var mongoUrl = _configuration.GetValue<string>("MongoDb:Url");
        var jobsDatabaseName = _configuration.GetValue<string>("MongoDb:JobsDatabaseName");
        var hangfireServerName = _configuration.GetValue<string>("Hangfire:ServerName");
        
        var mongoStorageOptions = new MongoStorageOptions
        {
            MigrationOptions = new MongoMigrationOptions
            {
                MigrationStrategy = new DropMongoMigrationStrategy(),
                BackupStrategy = new NoneMongoBackupStrategy(),
            },
            CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection,
            Prefix = "hangfire.mongo",
        };
        
        serviceCollection.AddHangfire(config =>
            config.UseMongoStorage(mongoUrl, jobsDatabaseName, mongoStorageOptions));

        serviceCollection.AddHangfireServer(serverOptions =>
        {
            serverOptions.ServerName = hangfireServerName;
        });
        
        return serviceCollection;
    }
}