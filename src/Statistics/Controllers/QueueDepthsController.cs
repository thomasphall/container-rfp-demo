using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace Statistics.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QueueDepthsController : ControllerBase
    {
        // GET api/QueueDepths/5
        [HttpGet("{queueName}")]
        public ActionResult<uint> Get(string queueName)
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
            var messageCount = response.MessageCount;

            return messageCount;
        }
    }
}