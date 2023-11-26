namespace Application.Sort;

public interface ISorter
{
    IOrderedQueryable<T> Sort<T>(IQueryable<T> unsorted, IEnumerable<SortingParameters> parameters) where T : ISortable;
}

