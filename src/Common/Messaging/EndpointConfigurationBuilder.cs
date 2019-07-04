using System;
using System.Threading.Tasks;
using Autofac;
using Common.Configuration;
using NServiceBus;
using NServiceBus.Logging;

namespace Common.Messaging
{
    public class EndpointConfigurationBuilder : IBuildEndpointConfigurations
    {
        private readonly IContainer _container;

        public EndpointConfigurationBuilder(IContainer container)
        {
            _container = container;
        }

        public EndpointConfiguration Build(string endpointName, string errorQueue = null, string auditQueue = null, int requestedConcurrency = 0)
        {
            var endpointConfiguration = GetBaseEndpointConfiguration(endpointName);
            ConfigureAuditing(auditQueue, endpointConfiguration);
            ConfigureCriticalErrorAction(endpointConfiguration);
            ConfigureDependencyInjection(endpointConfiguration);
            ConfigureErrorQueue(errorQueue, endpointConfiguration);
            ConfigureMaxConcurrency(requestedConcurrency, endpointConfiguration);
            ConfigureTransport(endpointConfiguration);

            return endpointConfiguration;
        }

        private void ConfigureAuditing(string auditQueue, EndpointConfiguration endpointConfiguration)
        {
            if (auditQueue != null)
            {
                endpointConfiguration.AuditProcessedMessagesTo(auditQueue);
            }
        }

        private void ConfigureCriticalErrorAction(EndpointConfiguration endpointConfiguration)
        {
            endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);
        }

        private void ConfigureDependencyInjection(EndpointConfiguration endpointConfiguration)
        {
            endpointConfiguration.UseContainer<AutofacBuilder>(customizations => customizations.ExistingLifetimeScope(_container));
        }

        private void ConfigureErrorQueue(string errorQueue, EndpointConfiguration endpointConfiguration)
        {
            if (errorQueue != null)
            {
                endpointConfiguration.SendFailedMessagesTo(errorQueue);
            }
        }

        private void ConfigureMaxConcurrency(int requestedConcurrency, EndpointConfiguration endpointConfiguration)
        {
            var maxConcurrency = GetMaxConcurrency(requestedConcurrency);
            endpointConfiguration.LimitMessageProcessingConcurrencyTo(maxConcurrency);
        }

        private void ConfigureTransport(EndpointConfiguration endpointConfiguration)
        {
            var configurationProvider = _container.Resolve<IProvideConfiguration>();
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.ConnectionString(configurationProvider.Configuration["rabbitmqConnectionString"]);
            transport.UseConventionalRoutingTopology();
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

        private int GetMaxConcurrency(int requestedConcurrency)
        {
            var maxConcurrency = Math.Max(requestedConcurrency, 1);
            var maxLogicalConcurrency = Math.Max(Environment.ProcessorCount, 2);
            return Math.Min(maxConcurrency, maxLogicalConcurrency);
        }

        private async Task OnCriticalError(ICriticalErrorContext context)
        {
            try
            {
                await context.Stop();
            }
            finally
            {
                Environment.FailFast($"Critical error, shutting down: {context.Error}", context.Exception);
            }
        }
    }
}