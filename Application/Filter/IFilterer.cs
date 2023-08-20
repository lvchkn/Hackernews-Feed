namespace Application.Filter;

public interface IFilterer
{
    IQueryable<T> Filter<T>(IQueryable<T> unfiltered, string? search) where T : IFilterable;
}