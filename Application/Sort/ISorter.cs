namespace Application.Sort;

public interface ISorter
{
    IQueryable<T> Sort<T>(IQueryable<T> unsorted, IEnumerable<SortingParameters> parameters) where T : ISortable;
}

