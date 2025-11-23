using Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Workers;

public class RankUpdater : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<RankUpdater> _logger;

    public RankUpdater(IServiceScopeFactory scopeFactory,
        ILogger<RankUpdater> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        _logger.LogInformation("Rank updateer executing");

        while (!token.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await dbContext.Database.ExecuteSqlRawAsync(@"
                UPDATE stories
                SET rank = score / POWER(EXTRACT(EPOCH FROM NOW()) - time + 3600, 1.8)
                WHERE time > (EXTRACT(EPOCH FROM (NOW() - INTERVAL '28 days')))
                AND score > 3;
            ", cancellationToken: token);

            int recomputedRanksCount = await dbContext.SaveChangesAsync(token);
            _logger.LogInformation("Ranks recomputed: {RanksCount}", recomputedRanksCount);

            await Task.Delay(TimeSpan.FromHours(2), token);
        }
    }
}
