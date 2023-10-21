using RabbitMQ.Client;

namespace Infrastructure.Rmq;

public class ChannelWrapper
{
    private readonly ThreadLocal<ChannelHolder> _channelHolder = new();

    public IModel? Channel
    {
        get => _channelHolder.Value?.Context;
        set
        {
            var holder = _channelHolder.Value;

            if (holder is not null)
            {
                holder.Context = null;
            }

            if (value is not null)
            {
                _channelHolder.Value = new ChannelHolder { Context = value };
            }
        }
    }

    private class ChannelHolder
    {
        public IModel? Context;
    }
}