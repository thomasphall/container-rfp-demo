using Common.RabbitMq;
using Microsoft.AspNetCore.Mvc;

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
            var queueDepthReader = new RabbitMqQueueDepthReader();
            var messageCount = queueDepthReader.GetQueueDepth(queueName);

            return messageCount;
        }
    }
}
