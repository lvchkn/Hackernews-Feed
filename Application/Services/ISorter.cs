using Application.Contracts;

namespace Application.Services
{
    public interface ISorter
    {
        List<CommentDto> Sort(IEnumerable<CommentDto> unsorted, SortingParameters parameters);
    }
}
