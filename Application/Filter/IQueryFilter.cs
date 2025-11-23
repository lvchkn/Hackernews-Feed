namespace Application.Filter;

public interface IQueryFilter<T> where T : class
{
    IQueryable<T> Filter(IQueryable<T> unfiltered, SearchCriteria search);
}