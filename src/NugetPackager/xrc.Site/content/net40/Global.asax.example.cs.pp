﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using Castle.MicroKernel.Registration;

// This is an example of a Global.asax.cs file that uses xrc and Windsor.

namespace $rootnamespace$
{
	public class MvcApplication : System.Web.HttpApplication
	{
        private static IWindsorContainer _container;

		protected void Application_Start()
		{
            BootstrapContainer();

			BootstrapXrc(RouteTable.Routes);

			RegisterRoutes(RouteTable.Routes);
        }

        private void BootstrapContainer()
        {
            _container = new WindsorContainer();

            // register xrc
			var xrcSection = xrc.Configuration.XrcSection.GetSection();
			_container.Install(new xrc.IoC.Windsor.XrcDefaultInstaller(xrcSection));

            // Register custom modules
            _container.Register(Classes.FromAssemblyContaining<MvcApplication>()
								.Where(p => p.Name.EndsWith("Module"))
                                .WithServiceSelf()
                                .WithServiceDefaultInterfaces()
								.LifestyleTransient());

            // Windsor MVC integration
            _container.Install(Castle.Windsor.Installer.FromAssembly.This());
            var controllerFactory = new WindsorControllerFactory(_container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }

		private void BootstrapXrc(RouteCollection routes)
		{
			xrc.IKernel kernel = _container.Resolve<xrc.IKernel>();
			kernel.Init();

			routes.Add("xrc", new xrc.Routing.XrcRoute());
		}

        private void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

			// Standard mvc route
			routes.MapRoute(
				 "Default", // Route name
				 "{controller}/{action}/{id}", // URL with parameters
				 new { controller = "Home", action = "Index", id = UrlParameter.Optional }); // Parameter defaults
		}
    }
}