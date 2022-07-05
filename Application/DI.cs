using Application.ApiConnections;
using Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DI
    {
        private static IConfiguration _configuration;
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            _configuration = configuration;

            services
                .AddHttp()
                .AddUtils()
                .AddApiConnection();

            return services;
        }

        private static IServiceCollection AddHttp(this IServiceCollection services)
        {
            var hackernewsApiUrl = _configuration.GetValue<string>("HackernewsApi:Url");

            services.AddHttpClient("ApiV0", options =>
                options.BaseAddress = new Uri(hackernewsApiUrl));

            return services;
        }

        private static IServiceCollection AddUtils(this IServiceCollection services)
        {
            services.AddScoped<ISorter, Sorter>();

            return services;
        }

        private static IServiceCollection AddApiConnection(this IServiceCollection services)
        {
            services.AddScoped<IApiConnector, ApiConnector>();

            return services;
        }
    }
}
