using Application.Contracts;

namespace Application.Services;

public enum SortOrder
{
    Asc, Desc
}

public enum SortField
{
    None, Id, Score, Title
}

public record SortingParameters(SortOrder Order, SortField FieldToSort);

public class Sorter : ISorter
{
    private readonly Dictionary<SortField, SortOrder> _sortingState = new()
    {
        [SortField.None] = SortOrder.Asc
    };

    public List<StoryDto> Sort(IEnumerable<StoryDto> unsorted, SortingParameters parameters)
    {
        var (sortOrder, fieldToSort) = parameters;

        if (!_sortingState.ContainsKey(fieldToSort))
        {
            _sortingState.Add(fieldToSort, sortOrder);
        }

        var index = 0;
        var listToSort = new List<StoryDto>(unsorted);

        foreach (var (field, order) in _sortingState)
        {
            var thenable = index > 1;
            index++;
            listToSort = GetSortedList(listToSort, field, order, thenable);
        }

        return listToSort;
    }

    private static List<StoryDto> GetSortedList(IEnumerable<StoryDto> list, SortField field, SortOrder order, bool thenable)
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

            case SortField.None:
                break;

            default: throw new ArgumentOutOfRangeException(nameof(field), field, null);
        }

        return listToOrder.ToList();
    }
}