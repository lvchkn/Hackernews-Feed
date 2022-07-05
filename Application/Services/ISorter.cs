using Domain.Entities;

namespace Application.Services
{
    public interface ISorter
    {
        List<Comment> Sort(IEnumerable<Comment> unsorted, SortingParameters parameters);
    }
}
