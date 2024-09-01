namespace Application.Sort;

public record SortingParameters(SortOrder Order, SortField FieldToSort);

public class SortingParametersParser
{
    public static List<SortingParameters> Parse(string? sortQuery)
    {
        if (string.IsNullOrEmpty(sortQuery)) 
        {
            return
            [
                new SortingParameters(SortOrder.Desc, SortField.Score)
            ];
        }

        var sortingParams = new List<SortingParameters>();
        var splitQuery = sortQuery.Trim().Split(',');

        foreach (var subSortQuery in splitQuery)
        {
            var splitSubSortQuery = subSortQuery.Trim().Split(" ");
            var sortOrder = SortOrder.Desc;
            var sortField = SortField.Score;

            if (splitSubSortQuery.Length > 0)
            {
                if (Enum.TryParse<SortField>(splitSubSortQuery[0], true, out var field))
                {
                    sortField = field;
                }
            }

            if (splitSubSortQuery.Length > 1)
            {
                if (Enum.TryParse<SortOrder>(splitSubSortQuery[1], true, out var order))
                {
                    sortOrder = order;
                }
            }

            sortingParams.Add(new SortingParameters(sortOrder, sortField));
        }
        
        return sortingParams;
    }
}

