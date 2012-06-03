﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using Castle.MicroKernel.Registration;

namespace DemoWebSite
{
	// TODO Valutare quale è la strada migliore: HttpHandler da web.config, route su controller, route su XrcRouteHandler

	public class MvcApplication : System.Web.HttpApplication
	{
        private IWindsorContainer _container;

		protected void Application_Start()
		{
            BootstrapContainer();

			RegisterRoutes(RouteTable.Routes);

            BootstrapXrc(RouteTable.Routes);
        }

        private void BootstrapContainer()
        {
            _container = new WindsorContainer();
            _container.Register(Component.For<xrc.Configuration.WorkingPath>().Instance(this.Server.MapPath("~/root")));
            _container.Register(Component.For<xrc.Configuration.XrcSection>().Instance(xrc.Configuration.XrcSection.GetSection()));
            _container.Install(new xrc.IoC.Windsor.XrcDefaultInstaller());

            _container.Register(Types.FromThisAssembly().BasedOn<xrc.Modules.IModule>());
        }

        private void RegisterRoutes(RouteCollection routes)
		{
            // Standard mvc route
			// Catch all the path starting with "mvc"
			routes.MapRoute(
			     "mvc_route",
			     "mvc/{controller}/{action}/{id}",
			     new { controller = "Home", action = "Index", id = UrlParameter.Optional });

			//routes.MapRoute(
			//     "Default", // Route name
			//     "{controller}/{action}/{id}", // URL with parameters
			//     new { controller = "Home", action = "Index", id = UrlParameter.Optional }); // Parameter defaults
		}

        private void BootstrapXrc(RouteCollection routes)
        {
            xrc.IKernel kernel = _container.Resolve<xrc.IKernel>();
            xrc.Kernel.Init(kernel);

            // xrc route
            // Catch all
            routes.Add("xrc_route", new Route("{*path}", new xrc.XrcRouteHandler()));

            //routes.MapRoute("xrc", "{*path}", new { controller = "XrcController", action = "DoWork" });
        }
    }
}