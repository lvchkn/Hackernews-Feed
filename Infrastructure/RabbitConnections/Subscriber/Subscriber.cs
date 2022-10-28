using System.Text;
using System.Text.Json;
using Application.Contracts;
using Application.Interfaces;
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

    public void Subscribe(string exchangeName, Func<StoryDto, Task> handle)
    {
        _channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);
        var queueName = _channel.QueueDeclare("stories", exclusive: false).QueueName;

        const string routingKey = "feed.story";
        _channel.QueueBind(queueName, exchangeName, routingKey);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, eventArgs) => 
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation("Story received: {0}", message);
            
            var jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var story = JsonSerializer.Deserialize<StoryDto>(message, jsonSerializerOptions);
            
            if (story is not null)
            {
                await handle(story);
                _logger.LogInformation("Story saved: {0}", story.ToString());
            }
        };

        _channel.BasicConsume(queueName, autoAck: true, consumer);
    }
}