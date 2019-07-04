using System;
using System.Threading.Tasks;
using Contracts;
using NServiceBus.Testing;
using NSubstitute;
using NUnit.Framework;
using Subscriber.MessageHandlers;

namespace Subscriber.Tests.EventMessageHandlerTests
{
    public class WhenAUniqueMessageIsHandled
    {
        private IDeleteEventMessages _eventMessageDeleter;
        private TestableMessageHandlerContext _messageHandlerContext;
        private EventMessage _eventMessage;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            _eventMessage = new EventMessage
            {
                MessageId = Guid.NewGuid()
            };

            _eventMessageDeleter = Substitute.For<IDeleteEventMessages>();
            _messageHandlerContext = new TestableMessageHandlerContext();
            await new EventMessageHandler(_eventMessageDeleter).Handle(_eventMessage, _messageHandlerContext);
        }

        [Test]
        public async Task TheMessageShouldBeDeletedFromTheDatabase()
        {
            await _eventMessageDeleter.Received(1).Delete(_eventMessage.MessageId);
        }
    }
}
