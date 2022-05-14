using HackerNewsCommentsFeed.Domain;

namespace HackerNewsCommentsFeed.RabbitConnections.Publisher;

public interface IPublisher
{
    void Publish<TMessage>(string exchangeName, TMessage message) where TMessage : class, IMessage;
}