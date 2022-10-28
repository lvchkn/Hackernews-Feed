using Application.Interfaces;
using Application.Services.Stories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Workers;

public class StoryFetcher : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ISubscriber _subscriber;

    public StoryFetcher(IServiceScopeFactory scopeFactory, ISubscriber subscriber)
    {
        _scopeFactory = scopeFactory;
        _subscriber = subscriber;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _subscriber.Subscribe("feed", async story => 
            {
                using var scope = _scopeFactory.CreateScope();
                var storiesService = scope.ServiceProvider.GetRequiredService<IStoriesService>();
            
                await storiesService.AddAsync(story);
            });
        }

        await Task.CompletedTask;
    }
}