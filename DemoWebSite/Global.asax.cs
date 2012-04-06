using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using xrc;

namespace DemoWebSite
{
	// TODO Valutare quale è la strada migliore: HttpHandler da web.config, route su controller, route su XrcRouteHandler

	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
            Kernel.Start(new DemoKernel(this.Server.MapPath("~/root")));

			RegisterRoutes(RouteTable.Routes);
		}

		protected virtual void RegisterRoutes(RouteCollection routes)
		{
            // Standard mvc route
			// Catch all the path starting with "mvc"
			routes.MapRoute(
			     "mvc_route",
			     "mvc/{controller}/{action}/{id}",
			     new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            // xrc route
			// Catch all
            routes.Add("xrc_route", new Route("{*path}", new XrcRouteHandler()));

			//routes.MapRoute("xrc", "{*path}", new { controller = "XrcController", action = "DoWork" });

			//routes.MapRoute(
			//     "Default", // Route name
			//     "{controller}/{action}/{id}", // URL with parameters
			//     new { controller = "Home", action = "Index", id = UrlParameter.Optional }); // Parameter defaults
		}
	}
}