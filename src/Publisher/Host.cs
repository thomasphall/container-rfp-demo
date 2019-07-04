using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Common.Configuration;
using Common.ConsoleSupport;
using Common.Data;
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

        public Host() : base(ENDPOINT_NAME, 10)
        {
            _eventMessageRecorder = new EventMessageRecorder(Container.Resolve<IProvideConfiguration>(), ENDPOINT_NAME);
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

        protected override void StartTasks()
        {
            StartMessagePump();
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

        private async Task PumpMessages()
        {
            try
            {
                await InnerPumpMessages().ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // For graceful shutdown purposes.
            }
            catch (Exception ex)
            {
                await ConsoleUtilities.WriteLineAsyncWithColor(ConsoleColor.Red, $"Error in message pump: {ex.Message}").ConfigureAwait(false);
            }

            if (!CancellationToken.IsCancellationRequested)
            {
                await PumpMessages().ConfigureAwait(false);
            }
        }

        private async Task InnerPumpMessages()
        {
            while (!CancellationTokenSource.IsCancellationRequested)
            {
                await Task.Delay(500, CancellationToken);
                var eventMessage = new EventMessage { MessageId = Guid.NewGuid() };
                await MessageSession.Publish(eventMessage, new PublishOptions()).ConfigureAwait(false);
                await _eventMessageRecorder.Record(eventMessage.MessageId);
                await ConsoleUtilities.WriteLineAsyncWithColor(ConsoleColor.Blue, $"Published message: {eventMessage.MessageId}");
            }

            await Task.CompletedTask;
        }
    }

    internal class EventMessageRecorder : EventMessageBase, IRecordEventMessages
    {
        private readonly string _clientName;

        public EventMessageRecorder(IProvideConfiguration configurationProvider, string clientName) : base(configurationProvider)
        {
            _clientName = clientName;
        }

        public async Task Record(Guid messageId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.spUnconsumedMessageInsert";
                var clientNameParameter = new SqlParameter("clientName", SqlDbType.VarChar, 100)
                {
                    Value = _clientName
                };
                var messageIdParameter = new SqlParameter("messageId", SqlDbType.UniqueIdentifier) { Value = messageId };

                command.Parameters.Add(clientNameParameter);
                command.Parameters.Add(messageIdParameter);
                await command.ExecuteNonQueryAsync();
                connection.Close();
            }
        }
    }

    internal interface IRecordEventMessages
    {
        Task Record(Guid eventMessageMessageId);
    }
}