using System;
using System.Threading.Tasks;
using Contracts;
using NServiceBus;
using NServiceBus.Testing;
using NSubstitute;
using NUnit.Framework;

namespace Subscriber.Tests.UniqueMessageHandlerTests
{
    public class WhenAUniqueMessageIsHandled
    {
        private IDeleteUnconsumedMessages _unconsumedMessageDeleter;
        private TestableMessageHandlerContext _messageHandlerContext;
        private UniqueMessage _uniqueMessage;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            _uniqueMessage = new UniqueMessage
            {
                MessageId = Guid.NewGuid()
            };

            _unconsumedMessageDeleter = Substitute.For<IDeleteUnconsumedMessages>();
            _messageHandlerContext = new TestableMessageHandlerContext();
            await new UniqueMessageHandler(_unconsumedMessageDeleter).Handle(_uniqueMessage, _messageHandlerContext);
        }

        [Test]
        public async Task TheMessageShouldBeDeletedFromTheDatabase()
        {
            await _unconsumedMessageDeleter.Received(1).Delete(_uniqueMessage.MessageId);
        }
    }
}
