// ---------------------------------------------------------------------------------------------------------------
// <copyright file="Host.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// Copyright 2019, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;

using Autofac;

using Common.Configuration;
using Common.ConsoleSupport;
using Common.Messaging;

using Contracts;

using NServiceBus;

namespace Publisher
{
    internal class Host : HostBase<PublisherModule>
    {
        private const string ENDPOINT_NAME = "Publisher";

        private readonly IRecordEventMessages _eventMessageRecorder;

        private Task _messagePumpTask;

        public Host()
            : base(ENDPOINT_NAME, 10)
        {
            _eventMessageRecorder = new EventMessageRecorder(Container.Resolve<IProvideConfiguration>(), ENDPOINT_NAME);
        }

        protected override void StartTasks()
        {
            StartMessagePump();
        }

        protected override async Task WaitForTasksToFinish()
        {
            var timeoutTask = Task.Delay(TimeSpan.FromSeconds(30), CancellationTokenSource.Token);

            var finishedTask = await Task.WhenAny(timeoutTask, _messagePumpTask).ConfigureAwait(false);

            if (finishedTask.Equals(timeoutTask))
            {
                await ConsoleUtilities.WriteLineAsyncWithColor(ConsoleColor.Red, "The message pump failed to stop with in the time allowed(30s)").ConfigureAwait(false);
            }
        }

        private async Task InnerPumpMessages()
        {
            while (!CancellationTokenSource.IsCancellationRequested)
            {
                await Task.Delay(1000, CancellationToken).ConfigureAwait(false);
                var eventMessage = new EventMessage { MessageId = Guid.NewGuid() };
                _eventMessageRecorder.Record(eventMessage.MessageId);
                await MessageSession.Publish(eventMessage, new PublishOptions()).ConfigureAwait(false);
                await ConsoleUtilities.WriteLineAsyncWithColor(ConsoleColor.Blue, $"Published message: {eventMessage.MessageId}").ConfigureAwait(false);
            }
        }

        private async Task PumpMessages()
        {
            try
            {
                if (!CancellationToken.IsCancellationRequested)
                {
                    await InnerPumpMessages().ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException)
            {
                // For graceful shutdown purposes.
            }
            catch (Exception ex)
            {
                await ConsoleUtilities.WriteLineAsyncWithColor(ConsoleColor.Red, $"Error in message pump: {ex.Message}").ConfigureAwait(false);
            }
        }

        private void StartMessagePump()
        {
            _messagePumpTask = Task.Factory
                                   .StartNew(
                                       PumpMessages,
                                       CancellationToken.None,
                                       TaskCreationOptions.LongRunning,
                                       TaskScheduler.Default)
                                   .Unwrap();
        }
    }
}
