namespace Application.Sort;

public static class SortingParametersParser
{
    public static List<SortParameters> Parse(string? sortQuery)
    {
        if (string.IsNullOrEmpty(sortQuery)) 
        {
            return
            [
                new SortParameters(SortOrder.Desc, SortField.Score)
            ];
        }

        var sortingParams = new List<SortParameters>();
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

            sortingParams.Add(new SortParameters(sortOrder, sortField));
        }
        
        return sortingParams;
    }
}

