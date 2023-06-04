﻿namespace Application.Sort;

public interface ISorter<T> where T : class
{
    List<T> Sort(IQueryable<T> unsorted, IEnumerable<SortingParameters> parameters);
}
