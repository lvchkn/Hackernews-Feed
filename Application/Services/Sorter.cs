using Domain.Entities;

namespace Application.Services;

public enum SortOrder
{
    Asc, Desc
}

public enum SortField
{
    None, Text, By
}

public record SortingParameters(SortOrder Order, SortField FieldToSort);

public class Sorter : ISorter
{
    private readonly Dictionary<SortField, SortOrder> _sortingState = new()
    {
        [SortField.None] = SortOrder.Asc
    };

    public List<Comment> Sort(IEnumerable<Comment> unsorted, SortingParameters parameters)
    {
        var (sortOrder, fieldToSort) = parameters;

        if (!_sortingState.ContainsKey(fieldToSort))
        {
            _sortingState.Add(fieldToSort, sortOrder);
        }

        var index = 0;
        var listToSort = new List<Comment>(unsorted);

        foreach (var (field, order) in _sortingState)
        {
            var thenable = index > 1;
            index++;
            listToSort = GetSortedList(listToSort, field, order, thenable);
        }

        return listToSort;
    }

    private static List<Comment> GetSortedList(IEnumerable<Comment> list, SortField field, SortOrder order, bool thenable)
    {
        var listToOrder = list.OrderBy(_ => true);

        switch (field)
        {
            case SortField.By when thenable && order == SortOrder.Asc:
                listToOrder = listToOrder.ThenBy(c => c.By);
                break;

            case SortField.By when thenable && order == SortOrder.Desc:
                listToOrder = listToOrder.ThenByDescending(c => c.By);
                break;

            case SortField.By when order == SortOrder.Asc:
                listToOrder = listToOrder.OrderBy(c => c.By);
                break;

            case SortField.By when order == SortOrder.Desc:
                listToOrder = listToOrder.OrderByDescending(c => c.By);
                break;

            case SortField.Text when thenable && order == SortOrder.Asc:
                listToOrder = listToOrder.ThenBy(c => c.Text);
                break;

            case SortField.Text when thenable && order == SortOrder.Desc:
                listToOrder = listToOrder.ThenByDescending(c => c.Text);
                break;

            case SortField.Text when order == SortOrder.Asc:
                listToOrder = listToOrder.OrderBy(c => c.Text);
                break;

            case SortField.Text when order == SortOrder.Desc:
                listToOrder = listToOrder.OrderByDescending(c => c.Text);
                break;

            case SortField.None:
                break;

            default: throw new ArgumentOutOfRangeException(nameof(field), field, null);
        }

        return listToOrder.ToList();
    }
}