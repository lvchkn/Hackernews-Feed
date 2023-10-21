using Application;
using Infrastructure;

namespace HackerNewsCommentsFeed.ServiceCollectionExtensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    { 
        services.AddApplication(configuration)
            .AddInfrastructure(configuration)
            .AddGithubAuth(configuration)
            .AddAuthorization()
            .AddCorsPolicy(configuration)
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();

        return services;
    }
}