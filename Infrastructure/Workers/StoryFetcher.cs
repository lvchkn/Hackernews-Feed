using System.Text;
using System.Text.Json;
using Application.Stories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure.Workers;

public class StoryFetcher : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConnectionFactory _connectionFactory;
    private readonly ILogger<StoryFetcher> _logger;

    public StoryFetcher(
        IServiceScopeFactory scopeFactory,
        IConnectionFactory connectionFactory,
        ILogger<StoryFetcher> logger)
    {
        _scopeFactory = scopeFactory;
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogWarning("Story fetcher executing");
        var connection = await _connectionFactory.CreateConnectionAsync(stoppingToken);
        var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        const string exchangeName = "feed";
        const string queueName = "stories";
        const string routingKey = "feed.stories";
        
        await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Topic, cancellationToken: stoppingToken);
        await channel.QueueDeclareAsync(queueName, exclusive: false, durable: true, cancellationToken: stoppingToken);
        await channel.QueueBindAsync(queueName, exchangeName, routingKey, cancellationToken: stoppingToken);
        await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 2, global: false, cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, eventArgs) => 
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation("Message received: {0}", message);
            
            var jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var story = JsonSerializer.Deserialize<StoryDto>(message, jsonSerializerOptions);
            
            if (story is not null)
            {
                using var scope = _scopeFactory.CreateScope();
                var storiesService = scope.ServiceProvider.GetRequiredService<IStoriesService>();

                await storiesService.AddAsync(story);
                _logger.LogInformation("Message saved: {0}", story.ToString());
            }
            
            await channel.BasicAckAsync(eventArgs.DeliveryTag, false, cancellationToken: stoppingToken);
        };
        
        await channel.BasicConsumeAsync(queueName, autoAck: false, consumer, cancellationToken: stoppingToken);
    }
}