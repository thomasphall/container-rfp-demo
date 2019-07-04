using Autofac;

namespace Subscriber.Registration
{
    internal class SqlClientModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterSqlCallers(builder);
        }

        private void RegisterSqlCallers(ContainerBuilder builder)
        {
            builder.RegisterType<EventMessageDeleter>().As<IDeleteEventMessages>().SingleInstance();
        }
    }
}
