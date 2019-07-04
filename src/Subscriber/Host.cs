using System;
using System.Threading.Tasks;
using Autofac;
using Common.Messaging;
using NServiceBus;
using NServiceBus.Logging;

namespace Subscriber
{
    internal class Host : HostBase<SubscriberModule>
    {
        private readonly EndpointConfiguration _endpointConfiguration;
        private IEndpointInstance _endpoint;
        private readonly ILog _log;

        public Host()
        {
            var endpointConfigurationBuilder = new EndpointConfigurationBuilder(Container);

            _log = LogManager.GetLogger<Host>();
            _endpointConfiguration = endpointConfigurationBuilder.Build(EndpointName, null, null, 10);
        }

        public static string EndpointName => "Subscriber";

        public async Task Start()
        {
            try
            {
                _endpoint = await Endpoint.Start(_endpointConfiguration);
            }
            catch (Exception ex)
            {
                FailFast("Failed to start.", ex);
            }
        }

        public async Task Stop()
        {
            try
            {
                await _endpoint?.Stop();
            }
            catch (Exception ex)
            {
                FailFast("Failed to stop correctly.", ex);
            }
        }

        private void FailFast(string message, Exception ex)
        {
            try
            {
                _log.Fatal(message, ex);
            }
            finally
            {
                Environment.FailFast(message, ex);
            }
        }
    }
}