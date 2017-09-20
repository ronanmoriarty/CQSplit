using System.Web.Mvc;
using System.Web.Routing;
using Cafe.Waiter.Web.Controllers;

namespace Cafe.Waiter.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Api",
                url: "api/{controller}/{action}",
                namespaces: new[] { typeof(Api.TabController).Namespace }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] {typeof(TabController).Namespace}
            );
        }
    }
}
