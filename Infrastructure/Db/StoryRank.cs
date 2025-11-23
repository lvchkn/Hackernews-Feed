using Application.Ranking;
using Domain.Entities;

namespace Infrastructure.Db;

public class StoryRank : IQueryRank<Story>
{
    private const double Gravity = 1.8;
    private const int SecondsInOneHour = 3600;

    public IEnumerable<Story> Rank(List<Story> stories)
    {
        foreach(var story in stories)
        {
            story.Rank = CalculateRank(story);
        }

        var adjustedEntities = stories
            .OrderByDescending(story => story.Rank);

        return adjustedEntities;
    }

    public double CalculateRank(Story story)
    {
        double rank = story.Score / Math.Pow(DateTimeOffset.UtcNow.ToUnixTimeSeconds() - story.Time + SecondsInOneHour, Gravity);
        return rank;
    }
}