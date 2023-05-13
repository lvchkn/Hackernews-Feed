using Application.Interfaces;
using Infrastructure.Mongo.Repositories;
using Infrastructure.RabbitConnections;
using Infrastructure.RabbitConnections.Publisher;
using Infrastructure.RabbitConnections.Subscriber;
using Infrastructure.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Infrastructure
{
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
            var rabbitHostname = _configuration?.GetValue<string>("RabbitMq:Hostname");
            var rabbitPort = _configuration?.GetValue<int>("RabbitMq:Port");
            var rabbitUsername = _configuration?.GetValue<string>("RabbitMq:Username");
            var rabbitPassword = _configuration?.GetValue<string>("RabbitMq:Password");

            services.AddSingleton(_ => new ConnectionFactory()
            {
                HostName = rabbitHostname,
                Port = rabbitPort ?? 5672,
                UserName = rabbitUsername,
                Password = rabbitPassword,
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
                options.UseNpgsql(_configuration?.GetConnectionString("Postgres")));            
            
            services.AddScoped<ICommentsRepository, CommentsRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IInterestsRepository, InterestsRepository>();
            services.AddScoped<IStoriesRepository, StoriesRepository>();

            return services;
        }
    }
}
