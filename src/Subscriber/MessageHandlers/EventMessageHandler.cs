using System.Threading.Tasks;
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
        }
    }
}