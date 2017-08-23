using System.Reflection;
using Cafe.Domain;
using Cafe.Domain.Events;
using Cafe.Waiter.Command.Service.Messaging;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CQRSTutorial.Core;
using CQRSTutorial.DAL;
using CQRSTutorial.Infrastructure;
using NHibernate;

namespace Cafe.Waiter.Command.Service
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
                    .WithServiceSelf()
                    .WithServiceAllInterfaces(),
                Classes
                    .FromAssemblyContaining<EnvironmentVariableMessageBusConfiguration>()
                    .InSameNamespaceAs<EnvironmentVariableMessageBusConfiguration>()
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
                    .For<ISessionFactory>()
                    .UsingFactoryMethod(kernel => kernel.Resolve<NHibernateConfiguration>().CreateSessionFactory())
                    .LifestyleSingleton()
                );
        }
    }
}