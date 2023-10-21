using RabbitMQ.Client;

namespace Infrastructure.Rmq;

public interface IChannelFactory
{
    IModel? Create();
}