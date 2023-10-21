using System.Text;
using System.Text.Json;
using Application.Messaging;
using RabbitMQ.Client;

namespace Infrastructure.Rmq.Publisher;

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
        const string routingKey = "feed.comment";

        var serializedMessageBody = JsonSerializer.Serialize(message);
        var messageBody = Encoding.UTF8.GetBytes(serializedMessageBody);

        _channel.BasicPublish(exchangeName, routingKey, body: messageBody);
    }
}