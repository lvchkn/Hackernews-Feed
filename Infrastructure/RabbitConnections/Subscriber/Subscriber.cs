using System.Text;
using System.Text.Json;
using Application.Messaging;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure.RabbitConnections.Subscriber;

public class Subscriber : ISubscriber
{
    private readonly IModel? _channel;
    private readonly ILogger<Subscriber> _logger;

    public Subscriber(
        IChannelFactory channelFactory,
        ILogger<Subscriber> logger)
    {
        _channel = channelFactory.Create();
        _logger = logger;
    }

    public void Subscribe<T>(
        string exchangeName,
        string queueName, 
        string routingKey, 
        Func<T, Task> handle) where T : IMessage, new()
    {
        _channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);
        _channel.QueueDeclare(queueName, exclusive: false);
        _channel.QueueBind(queueName, exchangeName, routingKey);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (model, eventArgs) => 
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation("Message received: {0}", message);
            
            var jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var dto = JsonSerializer.Deserialize<T>(message, jsonSerializerOptions);
            
            if (dto is not null)
            {
                await handle(dto);
                _logger.LogInformation("Message saved: {0}", dto.ToString());
            }

            await Task.Yield();
        };

        _channel.BasicConsume(queueName, autoAck: true, consumer);
    }
}