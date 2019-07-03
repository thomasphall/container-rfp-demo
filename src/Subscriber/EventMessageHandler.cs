using System;
using System.Threading.Tasks;
using Contracts;
using NServiceBus;

namespace Subscriber
{
    public class EventMessageHandler : IHandleMessages<EventMessage>
    {
        private readonly IDeleteUnconsumedMessages _unconsumedMessageDeleter;

        public EventMessageHandler(IDeleteUnconsumedMessages unconsumedMessageDeleter)
        {
            _unconsumedMessageDeleter = unconsumedMessageDeleter;
        }

        public async Task Handle(EventMessage eventMessage, IMessageHandlerContext context)
        {
            await _unconsumedMessageDeleter.Delete(eventMessage.MessageId);
        }
    }
}