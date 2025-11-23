namespace Application.Ranking;

public interface IQueryRank<T>
{
    IEnumerable<T> Rank(List<T> entities);
    double CalculateRank(T entity);
}