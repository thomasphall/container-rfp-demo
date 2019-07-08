using Autofac;
using Common.Registration;

namespace Common
{
    public class CommonModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterModules(builder);
        }

        private static void RegisterModules(ContainerBuilder builder)
        {
            builder.RegisterModule<ConfigurationModule>();
            builder.RegisterModule<RabbitMqModule>();
        }
    }
}
