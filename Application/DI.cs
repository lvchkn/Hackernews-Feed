using Application.Interests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Users;
using Application.Stories;
using Application.Tags;

namespace Application;

// ReSharper disable once InconsistentNaming
public static class DI
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var hackernewsApiUrl = configuration.GetValue<string>("HackernewsApi:Url");

        services.AddHttpClient("ApiV0", options =>
            options.BaseAddress = new Uri(hackernewsApiUrl ?? string.Empty));

        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IInterestsService, InterestsService>();
        services.AddScoped<IStoriesService, StoriesService>();

        services.AddSingleton<TagsCache>();
        services.AddScoped<ITagsService, TagsService>();

        return services;
    }
}