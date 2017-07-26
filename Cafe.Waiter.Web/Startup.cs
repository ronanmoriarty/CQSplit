using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Cafe.Waiter.Web.Startup))]
namespace Cafe.Waiter.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
