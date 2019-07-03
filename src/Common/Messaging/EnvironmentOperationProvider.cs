using System;
using NServiceBus.Logging;

namespace Common.Messaging
{
    public class EnvironmentOperationProvider : IProvideEnvironmentOperations
    {
        private readonly ILog _log;

        public EnvironmentOperationProvider(ILog log)
        {
            _log = log;
        }
        public void FailFast(string message, Exception exception)
        {
            try
            {
                _log.Fatal(message, exception);
            }
            finally
            {
                Environment.FailFast(message, exception);
            }
        }

    }
}
