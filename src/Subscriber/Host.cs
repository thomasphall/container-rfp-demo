using System;
using System.Threading.Tasks;
using Common.Messaging;
using Microsoft.Extensions.Configuration;
using NServiceBus;
using NServiceBus.Logging;

namespace Subscriber
{
    internal class Host
    {
        private readonly EndpointConfiguration _endpointConfiguration;
        private IEndpointInstance _endpoint;
        private readonly ILog _log;

        public Host()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var endpointConfigurationBuilder = new EndpointConfigurationBuilder(configuration);

            _log = LogManager.GetLogger<Host>();
            _endpointConfiguration = endpointConfigurationBuilder.Build(EndpointName, null, null, 10);
        }

        public string EndpointName => "Subscriber";

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