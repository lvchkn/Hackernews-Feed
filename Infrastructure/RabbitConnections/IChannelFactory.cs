using RabbitMQ.Client;

namespace Infrastructure.RabbitConnections;

public interface IChannelFactory
{
    IModel? Create();
}