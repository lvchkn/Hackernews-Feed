namespace Application.Services;

public class SortingParametersParser
{
    public List<SortingParameters> Parse(string? sortQuery)
    {
        if (string.IsNullOrEmpty(sortQuery)) 
        {
            return new List<SortingParameters>(0);
        }

        var sortingParams = new List<SortingParameters>();
        var splitQuery = sortQuery.Trim().Split(',');

        foreach (var subSortQuery in splitQuery)
        {
            var splitSubSortQuery = subSortQuery.Trim().Split(" ");
            var sortOrder = SortOrder.Asc;
            var sortField = SortField.None;

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

