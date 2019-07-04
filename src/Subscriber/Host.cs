using Common.Messaging;

namespace Subscriber
{
    internal class Host : HostBase<SubscriberModule>
    {
        private const string ENDPOINT_NAME = "Subscriber";

        public Host() : base(ENDPOINT_NAME, 10)
        {
        }
    }
}