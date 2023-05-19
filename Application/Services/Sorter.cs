using Domain.Entities;

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

public class StoriesSorter : ISorter<Story>
{
    public List<Story> Sort(IQueryable<Story> stories, IEnumerable<SortingParameters> parameters)
    {
        var index = 0;

        foreach (var (order, field) in parameters)
        {
            var thenable = index > 1;
            index++;
            stories = UpdateListOrder(stories, field, order, thenable);
        }

        return stories.ToList();
    }

    private static IQueryable<Story> UpdateListOrder(
        IQueryable<Story> list, 
        SortField field, 
        SortOrder order, 
        bool thenable)
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

        return listToOrder;
    }
}