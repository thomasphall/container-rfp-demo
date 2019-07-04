using System;
using System.Threading.Tasks;
using Common.ConsoleSupport;
using Contracts;
using NServiceBus;

namespace Subscriber.MessageHandlers
{
    public class EventMessageHandler : IHandleMessages<EventMessage>
    {
        private readonly IDeleteEventMessages _eventMessageDeleter;

        public EventMessageHandler(IDeleteEventMessages eventMessageDeleter)
        {
            _eventMessageDeleter = eventMessageDeleter;
        }

        public async Task Handle(EventMessage eventMessage, IMessageHandlerContext context)
        {
            await _eventMessageDeleter.Delete(eventMessage.MessageId);
            await ConsoleUtilities.WriteLineAsyncWithColor(ConsoleColor.Blue, $"Published message: {eventMessage.MessageId}");
            await Task.Delay(250).ConfigureAwait(false);
        }
    }
}