using Autofac;
using Common.RabbitMq;

namespace Common.Registration
{
    public class RabbitMqModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterRabbitMqTypes(builder);
        }

        private void RegisterRabbitMqTypes(ContainerBuilder builder)
        {
            builder.RegisterType<RabbitMqQueueDepthReader>().As<IGetQueueDepths>();
        }
    }
}