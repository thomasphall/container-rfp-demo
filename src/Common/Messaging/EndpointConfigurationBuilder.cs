using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

namespace Common.Messaging
{
    public class EndpointConfigurationBuilder : IBuildEndpointConfigurations
    {
        private readonly IProvideEnvironmentOperations _environmentOperationProvider;

        public EndpointConfigurationBuilder(IProvideEnvironmentOperations environmentOperationProvider)
        {
            _environmentOperationProvider = environmentOperationProvider;
        }

        public EndpointConfiguration Build(string endpointName, string errorQueue = null, string auditQueue = null, int requestedConcurrency = 0)
        {
            var endpointConfiguration = GetBaseEndpointConfiguration(endpointName);
            ConfigureAuditing(auditQueue, endpointConfiguration);
            ConfigureMaxConcurrency(requestedConcurrency, endpointConfiguration);
            ConfigureErrorQueue(errorQueue, endpointConfiguration);
            ConfigureCriticalErrorAction(endpointConfiguration);

            endpointConfiguration.UseTransport<RabbitMQTransport>();

            return endpointConfiguration;
        }

        private void ConfigureCriticalErrorAction(EndpointConfiguration endpointConfiguration)
        {
            endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);
        }

        private static void ConfigureErrorQueue(string errorQueue, EndpointConfiguration endpointConfiguration)
        {
            if (errorQueue != null)
            {
                endpointConfiguration.SendFailedMessagesTo(errorQueue);
            }
        }

        private static void ConfigureMaxConcurrency(int requestedConcurrency, EndpointConfiguration endpointConfiguration)
        {
            var maxConcurrency = GetMaxConcurrency(requestedConcurrency);
            endpointConfiguration.LimitMessageProcessingConcurrencyTo(maxConcurrency);
        }

        private static void ConfigureAuditing(string auditQueue, EndpointConfiguration endpointConfiguration)
        {
            if (auditQueue != null)
            {
                endpointConfiguration.AuditProcessedMessagesTo(auditQueue);
            }
        }

        private static int GetMaxConcurrency(int requestedConcurrency)
        {
            var maxConcurrency = Math.Max(requestedConcurrency, 1);
            var maxLogicalConcurrency = Math.Max(Environment.ProcessorCount, 2);
            return Math.Min(maxConcurrency, maxLogicalConcurrency);
        }

        private EndpointConfiguration GetBaseEndpointConfiguration(string endpointName)
        {
            var endpointConfiguration = new EndpointConfiguration(endpointName);
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            LogManager.Use<DefaultFactory>().Level(LogLevel.Info);

            return endpointConfiguration;
        }

        private async Task OnCriticalError(ICriticalErrorContext context)
        {
            try
            {
                await context.Stop();
            }
            finally
            {
                _environmentOperationProvider.FailFast($"Critical error, shutting down: {context.Error}", context.Exception);
            }
        }
    }
}