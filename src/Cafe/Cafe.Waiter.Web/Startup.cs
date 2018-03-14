using Cafe.Waiter.Queries.DAL;
using Cafe.Waiter.Web.Controllers;
using Cafe.Waiter.Web.Repositories;
using CQRSTutorial.DAL.Common;
using CQRSTutorial.Messaging;
using CQRSTutorial.Messaging.RabbitMq;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ISendEndpointProvider = CQRSTutorial.Messaging.ISendEndpointProvider;

namespace Cafe.Waiter.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Add(new ServiceDescriptor(typeof(IMenuRepository), typeof(MenuRepository), ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(typeof(IMenuConfiguration), typeof(MenuConfiguration), ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(typeof(IConnectionStringProvider), ReadModelConnectionStringProvider.Instance));
            services.Add(new ServiceDescriptor(typeof(ITabDetailsRepository), typeof(TabDetailsRepository), ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(typeof(IOpenTabsRepository), typeof(OpenTabsRepository), ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(typeof(IPlaceOrderCommandFactory), typeof(PlaceOrderCommandFactory), ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(typeof(ICommandSender), typeof(CommandSender), ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(typeof(ISendEndpointProvider), typeof(RabbitMqSendEndpointProvider), ServiceLifetime.Transient));
            var rabbitMqMessageBusFactory = new RabbitMqMessageBusFactory(new RabbitMqHostConfiguration(Configuration), NoReceiveEndpointsConfigurator.Instance);
            services.Add(new ServiceDescriptor(typeof(IBusControl), rabbitMqMessageBusFactory.Create()));
            services.Add(new ServiceDescriptor(typeof(IRabbitMqHostConfiguration), typeof(RabbitMqHostConfiguration), ServiceLifetime.Transient));
            services.AddSingleton(typeof(IConfigurationRoot), Configuration);
            services.Add(new ServiceDescriptor(typeof(ICommandSendConfiguration), typeof(CommandSendConfiguration), ServiceLifetime.Transient));

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }
            app.UseMvc();
            app.UseStaticFiles();
        }
    }
}
