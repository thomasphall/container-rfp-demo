using System;
using System.Threading.Tasks;
using Contracts;
using NServiceBus.Testing;
using NSubstitute;
using NUnit.Framework;

namespace Subscriber.Tests.EventMessageHandlerTests
{
    public class WhenAUniqueMessageIsHandled
    {
        private IDeleteUnconsumedMessages _unconsumedMessageDeleter;
        private TestableMessageHandlerContext _messageHandlerContext;
        private EventMessage _eventMessage;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            _eventMessage = new EventMessage
            {
                MessageId = Guid.NewGuid()
            };

            _unconsumedMessageDeleter = Substitute.For<IDeleteUnconsumedMessages>();
            _messageHandlerContext = new TestableMessageHandlerContext();
            await new EventMessageHandler(_unconsumedMessageDeleter).Handle(_eventMessage, _messageHandlerContext);
        }

        [Test]
        public async Task TheMessageShouldBeDeletedFromTheDatabase()
        {
            await _unconsumedMessageDeleter.Received(1).Delete(_eventMessage.MessageId);
        }
    }
}
