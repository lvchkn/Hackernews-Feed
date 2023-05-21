namespace Application.Messaging;

public interface IPublisher
{
    void Publish<TMessage>(string exchangeName, TMessage message) where TMessage : class, IMessage;
}