using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using System.Web.Mvc;
using xrc.HttpModules;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

[assembly: WebActivator.PreApplicationStartMethod(
    typeof(DemoWebSite.App_Start.XrcAppStart), "PreStart")]

// TODO Split this class:
// Currently this class has 4 responsability:
// - asp.net mvc castle windsor integration
// - xrc CustomErrorHttpModule registration
// - register demo web site modules on castle windsor
// - register xrc classes on castle windsor

namespace DemoWebSite.App_Start
{
	public class XrcAppStart
	{
		private static IWindsorContainer _container;

		public static void PreStart()
		{
			BootstrapContainer();

			BootstrapXrc(RouteTable.Routes);

			DynamicModuleUtility.RegisterModule(typeof(CustomErrorPages)); // .NET 4.0
			// HttpApplication.RegisterModule // .NET 4.5
		}

		static void BootstrapContainer()
		{
			_container = new WindsorContainer();

			// register xrc
			var xrcSection = xrc.Configuration.XrcSection.GetSection();
			_container.Install(new xrc.IoC.Windsor.XrcDefaultInstaller(xrcSection));

			// Register demo web site modules
			_container.Register(Classes.FromAssemblyContaining<TwitterModule>()
								.Where(p => p.Name.EndsWith("Module"))
								.WithServiceSelf()
								.WithServiceDefaultInterfaces()
								.LifestyleTransient());
			// Register demo web site views
			_container.Register(Classes.FromAssemblyContaining<DemoWebSiteView>()
					.BasedOn<xrc.Views.IView>()
					.WithServiceSelf() // currently views doesn't have an interface 
					.LifestyleTransient());

			// Windsor ASP.NET MVC integration
			_container.Install(Castle.Windsor.Installer.FromAssembly.This());
			var controllerFactory = new WindsorControllerFactory(_container.Kernel);
			ControllerBuilder.Current.SetControllerFactory(controllerFactory);
		}

		static void BootstrapXrc(RouteCollection routes)
		{
			xrc.IKernel kernel = _container.Resolve<xrc.IKernel>();
			kernel.Init();
		}
	}
}