using System;
using Cafe.Waiter.Contracts.Queries;
using Cafe.Waiter.Contracts.QueryResponses;
using Cafe.Waiter.Web.Controllers;
using Cafe.Waiter.Web.Messaging;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CQRSTutorial.Infrastructure;
using MassTransit;

namespace Cafe.Waiter.Web.DependencyInjection
{
    public class ControllerWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var controllerBasedOnDescriptor = Classes
                .FromAssemblyContaining<TabController>()
                .InSameNamespaceAs<TabController>()
                .WithServiceSelf()
                .WithServiceAllInterfaces();

            container.Register(
                Classes
                    .FromAssemblyContaining<IMessageBusFactory>()
                    .InSameNamespaceAs<IMessageBusFactory>()
                    .WithServiceSelf()
                    .WithServiceAllInterfaces()
                    .LifestyleTransient(),
                Classes
                    .FromThisAssembly()
                    .InSameNamespaceAs<RabbitMqMessageBusEndpointConfiguration>()
                    .WithServiceSelf()
                    .WithServiceAllInterfaces()
                    .LifestyleTransient(),
                SetControllerLifestyle(controllerBasedOnDescriptor),
                Component
                    .For<IBusControl>()
                    .UsingFactoryMethod(GetBusControl)
                    .LifestyleSingleton(),
                Component
                    .For<IRequestClient<IOpenTabsQuery, IOpenTabsQueryResponse>>()
                    .UsingFactoryMethod(CreateMessageRequestClient)
            );
        }

        private static MessageRequestClient<IOpenTabsQuery, IOpenTabsQueryResponse> CreateMessageRequestClient(IKernel kernel)
        {
            var messageBusConfiguration = kernel.Resolve<IMessageBusConfiguration>();
            var serviceAddress = new Uri($"{messageBusConfiguration.Uri.AbsoluteUri}open_tabs_query");
            Console.WriteLine($"Service address for TabController is: {serviceAddress}");
            return new MessageRequestClient<IOpenTabsQuery, IOpenTabsQueryResponse>(kernel.Resolve<IBusControl>(), serviceAddress, new TimeSpan(0,0,0,30));
        }

        private BasedOnDescriptor SetControllerLifestyle(BasedOnDescriptor controllerBasedOnDescriptor)
        {
            return ControllerLifestyleConfigurator.Instance.SetLifestyle(controllerBasedOnDescriptor);
        }

        private IBusControl GetBusControl(IKernel kernel)
        {
            var messageBusFactory = kernel.Resolve<RabbitMqMessageBusFactory>();
            return messageBusFactory.Create();
        }
    }
}