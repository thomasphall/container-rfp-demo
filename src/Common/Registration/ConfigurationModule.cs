using Autofac;
using Common.Configuration;

namespace Common.Registration
{
    public class ConfigurationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterConfigurationProviders(builder);
        }

        private static void RegisterConfigurationProviders(ContainerBuilder builder)
        {
            builder.RegisterType<AppSettingsConfigurationProvider>().As<IProvideConfiguration>().SingleInstance();
        }
    }
}
