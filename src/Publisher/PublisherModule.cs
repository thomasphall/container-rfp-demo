using Autofac;
using Common;
using Publisher.Registration;

namespace Publisher
{
    internal class PublisherModule : Module
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
