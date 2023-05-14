using Application.Contracts;

namespace Application.Services
{
    public interface ISorter
    {
        List<StoryDto> Sort(IEnumerable<StoryDto> unsorted, SortingParameters parameters);
    }
}
