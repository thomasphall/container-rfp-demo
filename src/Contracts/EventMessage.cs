using System;
using NServiceBus;

namespace Contracts
{
    [TimeToBeReceived("24:00:00")]
    public class EventMessage : IEvent
    {
        public Guid MessageId { get; set; }
    }
}
