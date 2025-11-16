using Application;
using Infrastructure;
using Microsoft.OpenApi;

namespace Api.ServiceCollectionExtensions;

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
            .AddSwaggerGen(options => options.SwaggerDoc("v1", new OpenApiInfo { Title = "HN Feed API", Version = "1.0"}));

        return services;
    }
}