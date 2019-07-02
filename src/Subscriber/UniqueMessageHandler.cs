using System;
using System.Threading.Tasks;
using Contracts;
using NServiceBus;

namespace Subscriber
{
    public class UniqueMessageHandler : IHandleMessages<UniqueMessage>
    {
        private readonly IDeleteUnconsumedMessages _unconsumedMessageDeleter;

        public UniqueMessageHandler(IDeleteUnconsumedMessages unconsumedMessageDeleter)
        {
            _unconsumedMessageDeleter = unconsumedMessageDeleter;
        }

        public async Task Handle(UniqueMessage uniqueMessage, IMessageHandlerContext context)
        {
            await _unconsumedMessageDeleter.Delete(uniqueMessage.MessageId);
        }
    }
}