// ---------------------------------------------------------------------------------------------------------------
// <copyright file="QueueDepthsController.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// © Copyright 2019, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------

using System.Threading.Tasks;

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
        public async Task<ActionResult<uint>> Get(string queueName)
        {
            var queueDepthReader = new RabbitMqQueueDepthReader();

            return await queueDepthReader.GetQueueDepth(queueName).ConfigureAwait(false);
        }
    }
}
