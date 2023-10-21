using Shared.Exceptions;

namespace Api.ServiceCollectionExtensions;

public static class CorsPolicy
{
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        var clientUrl = configuration.GetValue<string>("ClientUrl");
        
        if (clientUrl is null) throw new ConfigurationException("No allowed origins provided!");
        
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy => 
            {
                policy.WithOrigins(clientUrl)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }
}