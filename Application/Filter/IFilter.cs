namespace Application.Filter;

public interface IFilter<T>
{
    IQueryable<T> Filter(IQueryable<T> unfiltered, string? search);
}