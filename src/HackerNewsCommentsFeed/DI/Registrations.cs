using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using HackerNewsCommentsFeed.ApiConnections;
using HackerNewsCommentsFeed.RabbitConnections;
using HackerNewsCommentsFeed.RabbitConnections.Publisher;
using HackerNewsCommentsFeed.Repositories;
using HackerNewsCommentsFeed.Utils;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using RabbitMQ.Client;

namespace HackerNewsCommentsFeed.DI;

public static class Registrations
{
    private static IConfiguration? _configuration;
    public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        _configuration = configuration;
        
        services.AddHttp()
            .AddGithubAuth()
            .AddAuthorization()
            .AddCorsPolicies()
            .AddApiConnection()
            .AddHangfireWithMongoStorage()
            .AddRabbitConnection()
            .AddPublisher()
            .AddMongoDb()
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

    private static IServiceCollection AddRabbitConnection(this IServiceCollection services)
    {
        var rabbitHostname = _configuration.GetValue<string>("RabbitMq:Hostname");
        
        services.AddSingleton(_ => new ConnectionFactory()
        {
            HostName = rabbitHostname
        });
        
        services.AddSingleton<ChannelWrapper>();
        services.AddSingleton<IChannelFactory, ChannelFactory>();

        return services;
    }

    private static IServiceCollection AddPublisher(this IServiceCollection services)
    {
        services.AddSingleton<IPublisher, Publisher>();

        return services;
    }

    private static IServiceCollection AddApiConnection(this IServiceCollection services)
    {
        services.AddScoped<IApiConnector, ApiConnector>();

        return services;
    }

    private static IServiceCollection AddHttp(this IServiceCollection services)
    {
        var hackernewsApiUrl = _configuration.GetValue<string>("HackernewsApi:Url");
        
        services.AddHttpClient("ApiV0", options => 
            options.BaseAddress = new Uri(hackernewsApiUrl));

        return services;
    }

    private static IServiceCollection AddMongoDb(this IServiceCollection services)
    {
        services.Configure<MongoSettings>(_configuration?.GetSection("MongoDb"));
        services.AddSingleton<ICommentsRepository, CommentsRepository>();
        services.AddSingleton<IUsersRepository, UsersRepository>();
        
        return services;
    }
}