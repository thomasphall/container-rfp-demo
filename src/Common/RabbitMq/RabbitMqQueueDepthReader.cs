using RabbitMQ.Client;

namespace Common.RabbitMq
{
    public class RabbitMqQueueDepthReader : IGetQueueDepths
    {
        public uint GetQueueDepth(string queueName)
        {
            var connectionFactory = new ConnectionFactory
            {
                UserName = "admin",
                Password = "yourStrong(!)Password",
                VirtualHost = "/",
                HostName = "rabbitmq"
            };

            var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();
            var response = channel.QueueDeclarePassive(queueName);
            return response.MessageCount;
        }
    }
}