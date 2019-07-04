using Autofac;

namespace Publisher.Registration
{
    internal class SqlClientModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterSqlCallers(builder);
        }

        private void RegisterSqlCallers(ContainerBuilder builder)
        {
        }
    }
}
