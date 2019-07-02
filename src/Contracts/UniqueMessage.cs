using System;
using NServiceBus;

namespace Contracts
{
    public class UniqueMessage : IEvent
    {
        public Guid MessageId { get; set; }
    }
}
