using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Common.ConsoleSupport;
using Common.Registration;
using NServiceBus;
using NServiceBus.Logging;

namespace Common.Messaging
{
    public abstract class HostBase<TModule> : IProvideIoC
        where TModule : IModule, new()
    {
        private readonly ILog _log;
        private readonly uint _maxConnectionAttempts;
        private IEndpointInstance _endpoint;

        /// <summary>
        ///     Initializes a new instance of the <see cref="HostBase{TModule}" /> class.
        /// </summary>
        protected HostBase(string endpointName, uint maxConnectionAttempts)
        {
            CancellationTokenSource = new CancellationTokenSource();
            CancellationToken = CancellationTokenSource.Token;

            EndpointName = endpointName;
            Container = BuildContainer();

            _log = LogManager.GetLogger(GetType());
            _maxConnectionAttempts = maxConnectionAttempts;
        }

        protected CancellationToken CancellationToken { get; }

        protected CancellationTokenSource CancellationTokenSource { get; }

        public string EndpointName { get; }

        public IContainer Container { get; }

        protected IMessageSession MessageSession => _endpoint;

        private IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<TModule>();

            return builder.Build();
        }

        public async Task Start()
        {
            try
            {
                var connectionAttempts = 1;
                var endpointConfigurationBuilder = new EndpointConfigurationBuilder(Container);

                while (true)
                    try
                    {
                        var endpointConfiguration = endpointConfigurationBuilder.Build(EndpointName, null, null, 10);
                        _endpoint = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (connectionAttempts >= _maxConnectionAttempts) throw;

                        var delay = 100 * (int) Math.Pow(2, connectionAttempts++);
                        await ConsoleUtilities.WriteLineAsyncWithColor(ConsoleColor.Yellow, $"Startup failed, retry attempt {connectionAttempts} reason: {ex.Message}").ConfigureAwait(false);
                        await Task.Delay(delay).ConfigureAwait(false);
                    }
            }
            catch (Exception ex)
            {
                FailFast("Failed to start.", ex);
            }

            StartTasks();
        }

        public async Task Stop()
        {
            try
            {
                CancelTasks();
                await WaitForTasksToFinish();
                await _endpoint.Stop();
            }
            catch (Exception ex)
            {
                FailFast("Failed to stop correctly.", ex);
            }
        }

        private void CancelTasks()
        {
            CancellationTokenSource.Cancel();
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

        protected abstract Task WaitForTasksToFinish();

        protected abstract void StartTasks();
    }
}