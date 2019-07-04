using System.Threading.Tasks;
using Common.Messaging;

namespace Subscriber
{
    internal class Host : HostBase<SubscriberModule>
    {
        private const string ENDPOINT_NAME = "Subscriber";

        public Host() : base(ENDPOINT_NAME, 10)
        {
        }

        protected override Task WaitForTasksToFinish()
        {
            // Nothing to do.
            return Task.CompletedTask;
        }

        protected override void StartTasks()
        {
            // Nothing to do.
        }
    }
}