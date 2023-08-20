namespace Application.Sort;

public interface ISorter<T> where T : class
{
    IQueryable<T> Sort(IQueryable<T> unsorted, IEnumerable<SortingParameters> parameters);
}

