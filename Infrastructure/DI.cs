using Application.Interfaces;
using Infrastructure.RabbitConnections;
using Infrastructure.RabbitConnections.Publisher;
using Infrastructure.RabbitConnections.Subscriber;
using Infrastructure.Repositories;
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
            var connectionString = _configuration?.GetConnectionString("RabbitMq") ?? "";

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
                options.UseNpgsql(_configuration?.GetConnectionString("Postgres"));
            }); 
            
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IInterestsRepository, InterestsRepository>();
            services.AddScoped<IStoriesRepository, StoriesRepository>();

            return services;
        }
    }
}
