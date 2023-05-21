namespace Application.Messaging;

public interface ISubscriber
{
    void Subscribe<T>(
        string exchangeName,
        string queueName, 
        string routingKey, 
        Func<T, Task> handle) where T : IMessage, new();
}