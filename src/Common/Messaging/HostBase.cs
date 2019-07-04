using Autofac;
using Autofac.Core;
using Common.Registration;

namespace Common.Messaging
{
    public class HostBase<TModule> : IProvideIoC
    where TModule : IModule, new()
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="HostBase{TModule}" /> class.
        /// </summary>
        protected HostBase()
        {
            Container = BuildContainer();
        }

        /// <inheritdoc cref="IProvideIoC.Container" />
        public IContainer Container { get; }

        private IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<TModule>();

            return builder.Build();
        }
    }
}
