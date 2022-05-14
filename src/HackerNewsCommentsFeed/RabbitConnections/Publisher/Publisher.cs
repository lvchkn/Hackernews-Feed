using System.Text;
using System.Text.Json;
using HackerNewsCommentsFeed.Domain;
using RabbitMQ.Client;

namespace HackerNewsCommentsFeed.RabbitConnections.Publisher;

public class Publisher : IPublisher
{
    private readonly IModel? _channel;

    public Publisher(IChannelFactory channelFactory)
    {
        _channel = channelFactory.Create();
    }

    public void Publish<TMessage>(string exchangeName, TMessage message) where TMessage : class, IMessage
    {
        _channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);
        const string routingKey = "feed";

        var serializedMessageBody = JsonSerializer.Serialize(message);
        var messageBody = Encoding.UTF8.GetBytes(serializedMessageBody);
        
        _channel.BasicPublish(exchangeName, routingKey, body: messageBody);
    }
}