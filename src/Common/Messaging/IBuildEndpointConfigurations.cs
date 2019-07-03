using NServiceBus;

namespace Common.Messaging
{
    public interface IBuildEndpointConfigurations
    {
        EndpointConfiguration Build(string endpointName, string errorQueue = null, string auditQueue = null, int requestedConcurrency = 0);
    }
}