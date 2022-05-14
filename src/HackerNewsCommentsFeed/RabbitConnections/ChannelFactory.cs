using RabbitMQ.Client;

namespace HackerNewsCommentsFeed.RabbitConnections;

public class ChannelFactory : IChannelFactory
{
    private readonly IConnection _connection;
    private IModel? _channel;

    public ChannelFactory(ChannelWrapper channelWrapper, ConnectionFactory connectionFactory)
    {
        _connection = connectionFactory.CreateConnection();
        _channel = channelWrapper.Channel;
    }
    
    public IModel? Create()
    {
        if (_channel is null)
        {
            _channel = _connection.CreateModel();
            return _channel;
        }

        return _channel;
    }
}