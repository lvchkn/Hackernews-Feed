using Application.Services;
using Application.Services.Interests;
using Application.Services.Stories;
using Application.Services.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class DI
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            var hackernewsApiUrl = configuration.GetValue<string>("HackernewsApi:Url");

            services.AddHttpClient("ApiV0", options =>
                options.BaseAddress = new Uri(hackernewsApiUrl ?? string.Empty));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<ISorter, Sorter>();

            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IInterestsService, InterestsService>();
            services.AddScoped<IStoriesService, StoriesService>();

            return services;
        }
    }
}
