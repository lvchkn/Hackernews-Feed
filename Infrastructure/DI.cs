using Application.Filter;
using Application.Interests;
using Application.Sort;
using Application.Stories;
using Application.Users;
using Infrastructure.Db;
using Infrastructure.Db.Repositories;
using Infrastructure.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Infrastructure;

public static class DI
{
    private static IConfiguration? _configuration;
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        _configuration = configuration;
            
        services
            .AddRabbitMq()
            .AddRepos()
            .AddHostedService<StoryFetcher>();

        return services;
    }

    private static IServiceCollection AddRabbitMq(this IServiceCollection services)
    {
        var rmqUsername = _configuration?.GetValue<string>("RabbitMq:Username");
        var rmqPassword = _configuration?.GetValue<string>("RabbitMq:Password");
        var rmqHostname = _configuration?.GetValue<string>("RabbitMq:Hostname");
        var rmqPort = _configuration?.GetValue<string>("RabbitMq:Port");

        var connectionString = $"amqp://{rmqUsername}:{rmqPassword}@{rmqHostname}:{rmqPort}";
        services.AddSingleton<IConnectionFactory>(new ConnectionFactory
        {
            Uri = new Uri(connectionString),
            VirtualHost = "/",
        });

        return services;
    }

    private static IServiceCollection AddRepos(this IServiceCollection services)
    {
        var pgUsername = _configuration?.GetValue<string>("Postgres:Username");
        var pgPassword = _configuration?.GetValue<string>("Postgres:Password");
        var pgHost = _configuration?.GetValue<string>("Postgres:Host");
        var pgPort = _configuration?.GetValue<string>("Postgres:Port");
        var pgDbName = _configuration?.GetValue<string>("Postgres:Database");

        var connectionString = $"""
            Username={pgUsername};Password={pgPassword};
            Host={pgHost};Port={pgPort};Database={pgDbName};Include Error Detail=true
            """;

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention()
                .ConfigureWarnings(w =>
                    w.Default(WarningBehavior.Throw));
        }); 
            
        services.AddScoped<ISorter, Sorter>();
        services.AddScoped<IFilterer, Filterer>();

        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IInterestsRepository, InterestsRepository>();
        services.AddScoped<IStoriesRepository, StoriesRepository>();

        return services;
    }
}