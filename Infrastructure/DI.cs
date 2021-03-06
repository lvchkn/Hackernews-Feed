using Application.Interfaces;
using Domain.Entities;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Infrastructure.Mongo;
using Infrastructure.Mongo.Repositories;
using Infrastructure.RabbitConnections;
using Infrastructure.RabbitConnections.Publisher;
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
                .AddPublisher()
                .AddMongoDb()
                .AddRepos()
                .AddHangfireWithMongoStorage();

            return services;
        }

        private static IServiceCollection AddRabbitConnection(this IServiceCollection services)
        {
            var rabbitHostname = _configuration.GetValue<string>("RabbitMq:Hostname");
            var rabbitPort = _configuration.GetValue<int>("RabbitMq:Port");
            var rabbitUsername = _configuration.GetValue<string>("RabbitMq:Username");
            var rabbitPassword = _configuration.GetValue<string>("RabbitMq:Password");

            services.AddSingleton(_ => new ConnectionFactory()
            {
                HostName = rabbitHostname,
                Port = rabbitPort,
                UserName = rabbitUsername,
                Password = rabbitPassword
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

            services.Configure<MongoSettings>(_configuration?.GetSection("MongoDb"));

            return services;
        }

        private static IServiceCollection AddRepos(this IServiceCollection services)
        {
            services.AddScoped<ICommentsRepository, CommentsRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IInterestsRepository, InterestsRepository>();

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
}
