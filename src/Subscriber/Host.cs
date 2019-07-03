using System;
using System.Threading.Tasks;
using Common.Messaging;
using NServiceBus;
using NServiceBus.Logging;

namespace Subscriber
{
    internal class Host
    {
        private static readonly ILog Log = LogManager.GetLogger<Host>();
        private IEndpointInstance _endpoint;
        private EndpointConfiguration _endpointConfiguration;
        private readonly IProvideEnvironmentOperations _environmentOperationProvider;

        public Host()
        {
            _environmentOperationProvider = new EnvironmentOperationProvider(Log);
            BuildEndpointConfiguration();
        }

        public string EndpointName => "Subscriber";

        private void BuildEndpointConfiguration()
        {
            var endpointConfigurationBuilder = new EndpointConfigurationBuilder(_environmentOperationProvider);
            _endpointConfiguration = endpointConfigurationBuilder.Build(EndpointName, null, null, 10);
        }

        public async Task Start()
        {
            try
            {
                _endpoint = await Endpoint.Start(_endpointConfiguration);
            }
            catch (Exception ex)
            {
                _environmentOperationProvider.FailFast("Failed to start.", ex);
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
                _environmentOperationProvider.FailFast("Failed to stop correctly.", ex);
            }
        }
    }
}