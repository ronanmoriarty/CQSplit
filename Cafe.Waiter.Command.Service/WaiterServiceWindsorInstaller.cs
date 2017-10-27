using System.Collections.Generic;
using System.Reflection;
using Cafe.Waiter.Command.Service.Consumers;
using Cafe.Waiter.Domain;
using Cafe.Waiter.Events;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CQRSTutorial.Core;
using CQRSTutorial.DAL;
using CQRSTutorial.Messaging;

namespace Cafe.Waiter.Command.Service
{
    public class WaiterServiceWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes
                    .FromThisAssembly()
                    .InSameNamespaceAs<WaiterServiceWindsorInstaller>()
                    .WithServiceSelf()
                    .WithServiceAllInterfaces(),
                Classes
                    .FromThisAssembly()
                    .InSameNamespaceAs<OpenTabConsumer>()
                    .WithServiceSelf()
                    .WithServiceAllInterfaces(),
                Classes
                    .FromAssemblyContaining<IMessageBusFactory>()
                    .InSameNamespaceAs<IMessageBusFactory>()
                    .Unless(type => type == typeof(MessageBusEventPublisher))
                    .WithServiceSelf()
                    .WithServiceAllInterfaces(),
                Component
                    .For<Assembly>()
                    .Instance(typeof(TabOpened).Assembly)
                    .Named("assemblyForEventMapper"),
                Component
                    .For<EventMapper>()
                    .DependsOn(Dependency.OnComponent("assemblyContainingEvents", "assemblyForEventMapper")),
                Component
                    .For<IConnectionStringProviderFactory>()
                    .Instance(WriteModelConnectionStringProviderFactory.Instance),
                Classes
                    .FromAssemblyContaining<IUnitOfWorkFactory>()
                    .InSameNamespaceAs<IUnitOfWorkFactory>()
                    .Unless(type => type == typeof(CompositeEventStore) || type == typeof(EventHandler))
                    .WithServiceSelf()
                    .WithServiceAllInterfaces(),
                Component
                    .For<ICommandDispatcher>()
                    .ImplementedBy<CommandDispatcher>(),
                Component
                    .For<ICommandHandlerProvider>()
                    .UsingFactoryMethod(kernel =>
                    {
                        var commandHandlerProvider = new CommandHandlerProvider(kernel.Resolve<ICommandHandlerFactory>());
                        commandHandlerProvider.RegisterCommandHandler(new OpenTabCommandHandler());
                        return commandHandlerProvider;
                    }),
                Component
                    .For<TypeInspector>()
                    .ImplementedBy<TypeInspector>(),
                Component
                    .For<IEventApplier>()
                    .ImplementedBy<EventApplier>(),
                Component
                    .For<IEnumerable<IEventStore>>()
                    .UsingFactoryMethod(GetEventStores)
                    .Named("eventStores"),
                Component
                    .For<CompositeEventStore>()
                    .DependsOn(Dependency.OnComponent("eventStores", "eventStores"))
                    .Named("compositeEventStore"),
                Component
                    .For<IEventHandler>()
                    .ImplementedBy<EventHandler>()
                    .DependsOn(Dependency.OnComponent("eventStore", "compositeEventStore"))
                );
        }

        private IEnumerable<IEventStore> GetEventStores(IKernel kernel)
        {
            return new List<IEventStore>
            {
                kernel.Resolve<EventStore>(),
                kernel.Resolve<EventToPublishRepository>()
            };
        }
    }
}