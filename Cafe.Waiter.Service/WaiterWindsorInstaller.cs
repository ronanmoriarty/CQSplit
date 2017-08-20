using System.Collections.Generic;
using Cafe.Domain;
using Cafe.Waiter.Service.Messaging;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CQRSTutorial.Core;
using CQRSTutorial.Infrastructure;

namespace Cafe.Waiter.Service
{
    public class WaiterWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes
                    .FromThisAssembly()
                    .InSameNamespaceAs<WaiterWindsorInstaller>()
                    .WithServiceSelf()
                    .WithServiceAllInterfaces(),
                Classes
                    .FromThisAssembly()
                    .InSameNamespaceAs<MessageBusEndpointConfiguration>()
                    .Unless(type => type == typeof(OpenTabConsumer))
                    .WithServiceSelf()
                    .WithServiceAllInterfaces(),
                Classes
                    .FromAssemblyContaining<EnvironmentVariableMessageBusConfiguration>()
                    .InSameNamespaceAs<EnvironmentVariableMessageBusConfiguration>()
                    .WithServiceSelf()
                    .WithServiceAllInterfaces(),
                Component
                    .For<IEnumerable<ICommandHandler>>()
                    .Instance(new List<ICommandHandler> {new TabFactory()})
                    .Named("openTabConsumerCommandHandlers"),
                Component
                    .For<IAggregateStore>()
                    .ImplementedBy<AggregateStore>()
                    .DependsOn(Dependency.OnComponent("commandHandlers", "openTabConsumerCommandHandlers"))
                    .Named("openTabConsumerAggregateStore"),
                Component
                    .For<OpenTabConsumer>()
                    .DependsOn(Dependency.OnComponent("aggregateStore", "openTabConsumerAggregateStore"))
                );
        }
    }
}