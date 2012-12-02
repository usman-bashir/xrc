using System.Web.Mvc;
using Castle.Windsor;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using System.Web.Routing;

[assembly: WebActivator.PreApplicationStartMethod(
	typeof(xrc.Web.AcceptanceTest.xrcAppStart), "PreStart")]

namespace xrc.Web.AcceptanceTest
{
	public class xrcAppStart
	{
		static IWindsorContainer _container;

		public static void PreStart()
		{
			InitializeContainer();

			InitializeXrc();

			SetupHttpModulesFactory();

			SetupControllerFactory();

			RegisterRoutes(RouteTable.Routes);
		}

		static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("{*robotstxt}", new { robotstxt = @"(.*/)?robots.txt(/.*)?" });

			routes.Add("xrc", new xrc.Routing.XrcRoute());
		}

		static void InitializeContainer()
		{
			_container = new WindsorContainer();

			var xrcSection = xrc.Configuration.XrcSection.GetSection();
			_container.Install(new xrc.Windsor.Installers.XrcInstaller(xrcSection));

			_container.Install(Castle.Windsor.Installer.FromAssembly.This());
		}

		static void InitializeXrc()
		{
			xrc.IKernel kernel = _container.Resolve<xrc.IKernel>();
			kernel.Init();
		}

		static void SetupHttpModulesFactory()
		{
			xrc.Windsor.HttpModuleFactory.Setup(_container.Kernel);

			DynamicModuleUtility.RegisterModule(typeof(xrc.Windsor.HttpModuleFactory)); // .NET 4.0
			// HttpApplication.RegisterModule // .NET 4.5
		}

		static void SetupControllerFactory()
		{
			var controllerFactory = new xrc.Windsor.ControllerFactory(_container.Kernel);
			ControllerBuilder.Current.SetControllerFactory(controllerFactory);
		}
	}
}