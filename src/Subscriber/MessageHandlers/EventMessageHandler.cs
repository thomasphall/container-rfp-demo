// ---------------------------------------------------------------------------------------------------------------
// <copyright file="EventMessageHandler.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// © Copyright 2019, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------

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
            await Task.Delay(1000).ConfigureAwait(false);
            _eventMessageDeleter.Delete(eventMessage.MessageId);
            await ConsoleUtilities.WriteLineAsyncWithColor(ConsoleColor.Blue, $"Consumed message: {eventMessage.MessageId}");
        }
    }
}
