using Cafe.Waiter.Queries.DAL;
using Cafe.Waiter.Queries.DAL.Repositories;
using Cafe.Waiter.Web.Controllers;
using Cafe.Waiter.Web.Messaging;
using CQRSTutorial.DAL;
using CQRSTutorial.Messaging;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
            services.Add(new ServiceDescriptor(typeof(IConnectionStringProvider), ReadModelConnectionStringProviderFactory.Instance.GetConnectionStringProvider()));
            services.Add(new ServiceDescriptor(typeof(ITabDetailsRepository), typeof(TabDetailsRepository), ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(typeof(IOpenTabsRepository), typeof(OpenTabsRepository), ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(typeof(IPlaceOrderCommandFactory), typeof(PlaceOrderCommandFactory), ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(typeof(ICommandSender), typeof(CommandSender), ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(typeof(IEndpointProvider), typeof(EndpointProvider), ServiceLifetime.Transient));
            var rabbitMqMessageBusFactory = new RabbitMqMessageBusFactoryForConsuming(new EnvironmentVariableRabbitMqHostConfiguration(), new RabbitMqMessageBusEndpointConfiguration(), new ConsumerFactory());
            services.Add(new ServiceDescriptor(typeof(IBusControl), rabbitMqMessageBusFactory.Create()));
            services.Add(new ServiceDescriptor(typeof(IRabbitMqHostConfiguration), typeof(EnvironmentVariableRabbitMqHostConfiguration), ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(typeof(IServiceAddressProvider), typeof(ServiceAddressProvider), ServiceLifetime.Transient));
            services.AddSingleton(typeof(IConfigurationRoot), Configuration);

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
        }
    }
}
