public class RankingService : IRankingService
{
    private const double Gravity = 1.8;
    private const int SecondsInOneHour = 3600;

    private double CalculateRank(IRankable entity)
    {
        var rank = entity.Score / Math.Pow(DateTimeOffset.UtcNow.ToUnixTimeSeconds() - entity.Time + SecondsInOneHour, Gravity);
        return rank;
    }

    public List<T> Rank<T>(IEnumerable<T> entities) where T : IRankable
    {
        foreach(var entity in entities)
        {
            entity.Rank = CalculateRank(entity);
        }

        var adjustedEntities = entities
            .OrderByDescending(story => story.Rank)
            .ToList();
        
        return adjustedEntities;
    }
}
