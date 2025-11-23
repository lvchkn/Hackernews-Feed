namespace Application.Sort;

public interface IQuerySort<T> where T: class
{
    IOrderedQueryable<T> Sort(IQueryable<T> unsorted, IEnumerable<SortParameters> parameters);
}

