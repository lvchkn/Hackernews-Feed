using Application.Contracts;

namespace Application.Interfaces;

public interface ISubscriber
{
    void Subscribe(string exchangeName, Func<StoryDto, Task> handle);
}