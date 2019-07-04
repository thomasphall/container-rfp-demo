using Autofac;
using Common;
using Subscriber.Registration;

namespace Subscriber
{
    internal class SubscriberModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterModules(builder);
        }

        private static void RegisterModules(ContainerBuilder builder)
        {
            builder.RegisterModule<CommonModule>();
            builder.RegisterModule<SqlClientModule>();
        }
    }
}
