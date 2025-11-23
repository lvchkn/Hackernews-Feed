using Application.Sort;
using Domain.Entities;

namespace Infrastructure.Db;

public class StorySort : IQuerySort<Story>
{
    public IOrderedQueryable<Story> Sort(IQueryable<Story> entities, IEnumerable<SortParameters> parameters)
    {
        var @params = parameters.ToList();
        IOrderedQueryable<Story>? orderedEntities = ApplyPrimarySort(entities, @params[0].FieldToSort, @params[0].Order);
        
        for (var i = 1; i < @params.Count; i++)
        {
            orderedEntities = ApplySecondarySort(orderedEntities, @params[i].FieldToSort, @params[i].Order);
        }

        return orderedEntities;
    }

    private static IOrderedQueryable<Story> ApplyPrimarySort(IQueryable<Story> list, SortField field, SortOrder order)
    {
        switch (field)
        {
            case SortField.Id when order == SortOrder.Asc:
                return list.OrderBy(s => s.Id);
            case SortField.Id when order == SortOrder.Desc:
                return list.OrderByDescending(s => s.Id);

            case SortField.Score when order == SortOrder.Asc:
                return list.OrderBy(s => s.Score);
            case SortField.Score when order == SortOrder.Desc:
                return list.OrderByDescending(s => s.Score);
                    
            case SortField.Title when order == SortOrder.Asc:
                return list.OrderBy(s => s.Title);                
            case SortField.Title when order == SortOrder.Desc:
                return list.OrderByDescending(s => s.Title);
            
            default:
                throw new ArgumentOutOfRangeException(nameof(field), field, "Unexpected SortField argument value");
        }
    }
    
    private static IOrderedQueryable<Story> ApplySecondarySort(IOrderedQueryable<Story> list, SortField field, SortOrder order)
    {
        switch (field)
        {
            case SortField.Id when order == SortOrder.Asc:
                return list.ThenBy(s => s.Id);
            case SortField.Id when order == SortOrder.Desc:
                return list.ThenByDescending(s => s.Id);

            case SortField.Score when order == SortOrder.Asc:
                return list.ThenBy(s => s.Score);
            case SortField.Score when order == SortOrder.Desc:
                return list.ThenByDescending(s => s.Score);
                    
            case SortField.Title when order == SortOrder.Asc:
                return list.ThenBy(s => s.Title);                
            case SortField.Title when order == SortOrder.Desc:
                return list.ThenByDescending(s => s.Title);
            
            default:
                throw new ArgumentOutOfRangeException(nameof(field), field, "Unexpected SortField argument value");
        }
    }
}