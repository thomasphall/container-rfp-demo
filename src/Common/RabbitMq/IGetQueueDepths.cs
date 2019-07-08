namespace Common.RabbitMq
{
    public interface IGetQueueDepths
    {
        uint GetQueueDepth(string queueName);
    }
}