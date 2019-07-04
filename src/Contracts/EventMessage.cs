using System;
using NServiceBus;

namespace Contracts
{
    [TimeToBeReceived("00:01:00")]
    public class EventMessage : IEvent
    {
        public Guid MessageId { get; set; }
    }
}
