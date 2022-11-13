using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Mongo;
using Infrastructure.Mongo.Repositories;
using Infrastructure.RabbitConnections;
using Infrastructure.RabbitConnections.Publisher;
using Infrastructure.RabbitConnections.Subscriber;
using Infrastructure.Workers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
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
                .AddMongoDb()
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

        private static IServiceCollection AddMongoDb(this IServiceCollection services)
        {
            BsonClassMap.RegisterClassMap<Comment>(cm =>
            {
                cm.AutoMap();
                cm.GetMemberMap(c => c.Id).SetIgnoreIfDefault(true);
                cm.SetIdMember(cm.GetMemberMap(c => c.Id));
                cm.IdMemberMap.SetIdGenerator(StringObjectIdGenerator.Instance);
                cm.IdMemberMap.SetSerializer(new StringSerializer(BsonType.String));
            });

            BsonClassMap.RegisterClassMap<Interest>(cm =>
            {
                cm.AutoMap();
                cm.GetMemberMap(c => c.Id).SetIgnoreIfDefault(true);
                cm.SetIdMember(cm.GetMemberMap(c => c.Id));
                cm.IdMemberMap.SetIdGenerator(StringObjectIdGenerator.Instance);
                cm.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
            });

            BsonClassMap.RegisterClassMap<User>(cm =>
            {
                cm.AutoMap();
                cm.GetMemberMap(c => c.Id).SetIgnoreIfDefault(true);
                cm.SetIdMember(cm.GetMemberMap(c => c.Id));
                cm.IdMemberMap.SetIdGenerator(StringObjectIdGenerator.Instance);
                cm.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
            });

            BsonClassMap.RegisterClassMap<Story>(cm =>
            {
                cm.AutoMap();
                cm.GetMemberMap(c => c.Id).SetIgnoreIfDefault(true);
                cm.SetIdMember(cm.GetMemberMap(c => c.Id));
                cm.IdMemberMap.SetIdGenerator(StringObjectIdGenerator.Instance);
                cm.IdMemberMap.SetSerializer(new StringSerializer(BsonType.String));
            });

            services.Configure<MongoSettings>(_configuration?.GetSection("MongoDb")!);

            return services;
        }

        private static IServiceCollection AddRepos(this IServiceCollection services)
        {
            services.AddScoped<ICommentsRepository, CommentsRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IInterestsRepository, InterestsRepository>();
            services.AddScoped<IStoriesRepository, StoriesRepository>();

            return services;
        }
    }
}
