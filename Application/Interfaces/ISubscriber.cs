using Application.Contracts;

namespace Application.Interfaces;

public interface ISubscriber
{
    void Subscribe<T>(
        string exchangeName,
        string queueName, 
        string routingKey, 
        Func<T, Task> handle) where T : IMessage, new();
}