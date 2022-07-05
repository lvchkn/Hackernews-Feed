namespace Application.Interfaces;

public interface IPublisher
{
    void Publish<TMessage>(string exchangeName, TMessage message) //where TMessage : class, IMessage;
}