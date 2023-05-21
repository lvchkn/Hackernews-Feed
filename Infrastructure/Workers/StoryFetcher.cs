using Application.Messaging;
using Application.Stories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Workers;

public class StoryFetcher : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ISubscriber _subscriber;
    private readonly ILogger<StoryFetcher> _logger;

    public StoryFetcher(
        IServiceScopeFactory scopeFactory,
        ISubscriber subscriber,
        ILogger<StoryFetcher> logger)
    {
        _scopeFactory = scopeFactory;
        _subscriber = subscriber;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogWarning("Story fetcher executing");

        _subscriber.Subscribe<StoryDto>("feed", "stories", "feed.stories", async story =>
        {
            using var scope = _scopeFactory.CreateScope();
            var storiesService = scope.ServiceProvider.GetRequiredService<IStoriesService>();

            await storiesService.AddAsync(story);
        });

        await Task.CompletedTask;
    }
}