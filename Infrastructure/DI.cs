using Application.Filter;
using Application.Interests;
using Application.Messaging;
using Application.Sort;
using Application.Stories;
using Application.Users;
using Domain.Entities;
using Infrastructure.Db;
using Infrastructure.Db.Repositories;
using Infrastructure.RabbitConnections;
using Infrastructure.RabbitConnections.Publisher;
using Infrastructure.RabbitConnections.Subscriber;
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
            .AddRabbitConnection()
            .AddRepos()
            .AddHostedService<StoryFetcher>();

        return services;
    }

    private static IServiceCollection AddRabbitConnection(this IServiceCollection services)
    {
        var connectionString = _configuration?.GetConnectionString("RabbitMq")
            ?? Environment.GetEnvironmentVariable("RMQ_URI")
            ?? "";

        services.AddSingleton(_ => new ConnectionFactory()
        {
            Uri = new Uri(connectionString),
            DispatchConsumersAsync = true,
        });

        services.AddSingleton<ChannelWrapper>();
        services.AddSingleton<IChannelFactory, ChannelFactory>();

        services.AddSingleton<IPublisher, Publisher>();
        services.AddSingleton<ISubscriber, Subscriber>();

        return services;
    }

    private static IServiceCollection AddRepos(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            var connectionString = _configuration?.GetConnectionString("Postgres") 
                ?? Environment.GetEnvironmentVariable("POSTGRES_CONNSTRING")
                ?? "";

            options.UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention();
        }); 
            
        services.AddScoped<ISorter<Story>, StoriesSorter>();
        services.AddScoped<IFilter<Story>, StoriesFilter>();

        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IInterestsRepository, InterestsRepository>();
        services.AddScoped<IStoriesRepository, StoriesRepository>();

        return services;
    }
}