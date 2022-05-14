using RabbitMQ.Client;

namespace HackerNewsCommentsFeed.RabbitConnections;

public interface IChannelFactory
{
    IModel? Create();
}