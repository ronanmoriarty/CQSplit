using System;
using System.Collections.Generic;
using System.Reflection;
using Cafe.DAL.Common;
using Cafe.DAL.Sql;
using Cafe.Waiter.Command.Service.Consumers;
using Cafe.Waiter.Domain;
using Cafe.Waiter.Events;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CQSplit.Core;
using CQSplit.DAL;
using CQSplit.Messaging;
using CQSplit.Messaging.RabbitMq;
using CQSplit.Publish;
using MassTransit;
using Microsoft.Extensions.Configuration;
using ConfigurationRoot = Cafe.DAL.Common.ConfigurationRoot;
using EventHandler = CQSplit.DAL.EventHandler;

namespace Cafe.Waiter.Command.Service
{
    public class WaiterCommandServiceWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes
                    .FromAssembly(Assembly.GetExecutingAssembly())
                    .InSameNamespaceAs<WaiterCommandServiceWindsorInstaller>()
                    .WithServiceSelf()
                    .WithServiceAllInterfaces(),
                Classes
                    .FromAssembly(Assembly.GetExecutingAssembly())
                    .InSameNamespaceAs<OpenTabConsumer>()
                    .WithServiceSelf()
                    .WithServiceAllInterfaces(),
                Classes
                    .FromAssemblyContaining<IMessageBusFactory>()
                    .InSameNamespaceAs<IMessageBusFactory>()
                    .Unless(IsMessagingTypeNotRequiredInCommandService)
                    .WithServiceSelf()
                    .WithServiceAllInterfaces(),
                Classes
                    .FromAssemblyContaining<RabbitMqMessageBusFactory>()
                    .InSameNamespaceAs<RabbitMqMessageBusFactory>()
                    .Unless(IsMessagingRabbitMqTypeNotRequiredInCommandService)
                    .WithServiceSelf()
                    .WithServiceAllInterfaces(),
                Component
                    .For<Assembly>()
                    .Instance(typeof(TabOpened).Assembly)
                    .Named("assemblyForEventMapper"),
                Component
                    .For<EventSerializer>()
                    .DependsOn(Dependency.OnComponent("assemblyContainingEvents", "assemblyForEventMapper")),
                Classes
                    .FromAssemblyContaining<EventToPublishSerializer>()
                    .InSameNamespaceAs<EventToPublishSerializer>()
                    .Unless(type => type == typeof(CompositeEventStore) || type == typeof(EventHandler))
                    .WithServiceSelf()
                    .WithServiceAllInterfaces(),
                Classes
                    .FromAssemblyContaining<EventRepository>()
                    .InSameNamespaceAs<EventRepository>()
                    .Unless(type => type == typeof(EventStoreUnitOfWork))
                    .WithServiceSelf()
                    .WithServiceAllInterfaces(),
                Component
                    .For<IUnitOfWork>()
                    .UsingFactoryMethod(kernel =>
                    {
                        var connectionString = ConfigurationRoot.Instance["connectionString"];
                        return new EventStoreUnitOfWork(connectionString);
                    })
                    .LifestyleSingleton(),
                Component
                    .For<ICommandRouter>()
                    .ImplementedBy<CommandRouter>(),
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
                    .For<EventHandler>()
                    .DependsOn(Dependency.OnComponent("eventStore", "compositeEventStore")),
                Component
                    .For<OutboxToMessageBusEventHandler>(),
                Classes
                    .FromAssemblyContaining<OutboxToMessageBusPublisher>()
                    .InSameNamespaceAs<OutboxToMessageBusPublisher>()
                    .WithServiceSelf()
                    .WithServiceAllInterfaces(),
                Component
                    .For<IBusControl>()
                    .UsingFactoryMethod(GetBus)
                    .LifestyleSingleton(),
                Component
                    .For<IEnumerable<IEventHandler>>()
                    .UsingFactoryMethod(GetEventHandlers)
                    .Named("eventHandlers"),
                Component
                    .For<IEventHandler>()
                    .ImplementedBy<CompositeEventHandler>()
                    .DependsOn(Dependency.OnComponent("eventHandlers", "eventHandlers")),
                Component
                    .For<IConfigurationRoot>()
                    .Instance(ConfigurationRoot.Instance)
                );
        }

        private IBusControl GetBus(IKernel kernel)
        {
            var messageBusFactory = kernel.Resolve<IMessageBusFactory>();
            return new MultipleConnectionAttemptMessageBusFactory(messageBusFactory).Create();
        }

        private IEnumerable<IEventHandler> GetEventHandlers(IKernel kernel)
        {
            return new List<IEventHandler>
            {
                kernel.Resolve<EventHandler>(),
                kernel.Resolve<OutboxToMessageBusEventHandler>()
            };
        }

        private bool IsMessagingTypeNotRequiredInCommandService(Type type)
        {
            return type == typeof(InMemoryReceiveEndpointsConfigurator)
                || type == typeof(InMemoryMessageBusFactory);
        }

        private bool IsMessagingRabbitMqTypeNotRequiredInCommandService(Type type)
        {
            return type == typeof(NoReceiveEndpointsConfigurator)
                || type == typeof(MultipleConnectionAttemptMessageBusFactory);
        }

        private IEnumerable<IEventStore> GetEventStores(IKernel kernel)
        {
            var unitOfWork = kernel.Resolve<IUnitOfWork>();
            var eventRepository = kernel.Resolve<EventRepository>();
            eventRepository.UnitOfWork = unitOfWork;
            var eventToPublishRepository = kernel.Resolve<EventToPublishRepository>();
            eventToPublishRepository.UnitOfWork = unitOfWork;
            return new List<IEventStore>
            {
                eventRepository,
                eventToPublishRepository
            };
        }
    }
}