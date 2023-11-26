using Application.Sort;

namespace Infrastructure.Db;

public class Sorter : ISorter
{
    public IOrderedQueryable<T> Sort<T>(IQueryable<T> entities, IEnumerable<SortingParameters> parameters) where T : ISortable
    {
        var index = 0;

        foreach (var (order, field) in parameters)
        {
            var thenable = index > 1;
            index++;
            entities = UpdateListOrder(entities, field, order, thenable);
        }

        return entities as IOrderedQueryable<T> ?? throw new InvalidCastException();
    }

    private static IOrderedQueryable<T> UpdateListOrder<T>(
        IQueryable<T> list, 
        SortField field, 
        SortOrder order, 
        bool thenable) where T : ISortable
    {
        var listToOrder = list.OrderBy(_ => true);

        switch (field)
        {
            case SortField.Id when thenable && order == SortOrder.Asc:
                listToOrder = listToOrder.ThenBy(c => c.Id);
                break;

            case SortField.Id when thenable && order == SortOrder.Desc:
                listToOrder = listToOrder.ThenByDescending(c => c.Id);
                break;

            case SortField.Id when order == SortOrder.Asc:
                listToOrder = listToOrder.OrderBy(c => c.Id);
                break;

            case SortField.Id when order == SortOrder.Desc:
                listToOrder = listToOrder.OrderByDescending(c => c.Id);
                break;


            case SortField.Score when thenable && order == SortOrder.Asc:
                listToOrder = listToOrder.ThenBy(c => c.Score);
                break;

            case SortField.Score when thenable && order == SortOrder.Desc:
                listToOrder = listToOrder.ThenByDescending(c => c.Score);
                break;

            case SortField.Score when order == SortOrder.Asc:
                listToOrder = listToOrder.OrderBy(c => c.Score);
                break;

            case SortField.Score when order == SortOrder.Desc:
                listToOrder = listToOrder.OrderByDescending(c => c.Score);
                break;


            case SortField.Title when thenable && order == SortOrder.Asc:
                listToOrder = listToOrder.ThenBy(c => c.Title);
                break;

            case SortField.Title when thenable && order == SortOrder.Desc:
                listToOrder = listToOrder.ThenByDescending(c => c.Title);
                break;

            case SortField.Title when order == SortOrder.Asc:
                listToOrder = listToOrder.OrderBy(c => c.Title);
                break;

            case SortField.Title when order == SortOrder.Desc:
                listToOrder = listToOrder.OrderByDescending(c => c.Title);
                break;

            default: throw new ArgumentOutOfRangeException(nameof(field), field, null);
        }

        return listToOrder;
    }
}