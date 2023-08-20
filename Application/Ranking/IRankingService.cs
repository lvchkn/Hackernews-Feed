public interface IRankingService
{
    List<T> Rank<T>(IEnumerable<T> entities) where T : IRankable;
}