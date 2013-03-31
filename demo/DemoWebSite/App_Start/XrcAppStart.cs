using System.Web.Mvc;
using Castle.Windsor;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using System.Web.Routing;

[assembly: WebActivator.PreApplicationStartMethod(
	typeof(DemoWebSite.xrcAppStart), "PreStart")]

namespace DemoWebSite
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

            xrc.XrcWindsor.InstallCore(_container, xrcSection);

			_container.Install(Castle.Windsor.Installer.FromAssembly.This());
		}

		static void InitializeXrc()
		{
			xrc.IKernel kernel = _container.Resolve<xrc.IKernel>();
			kernel.Init();
		}

		static void SetupHttpModulesFactory()
		{
			xrc.Web.WindsorHttpModuleFactory.Setup(_container.Kernel);

            // .NET 4.0
            // DynamicModuleUtility.RegisterModule(typeof(xrc.Web.WindsorHttpModuleFactory)); 
            System.Web.HttpApplication.RegisterModule(typeof(xrc.Web.WindsorHttpModuleFactory));
        }

		static void SetupControllerFactory()
		{
			var controllerFactory = new xrc.Web.WindsorControllerFactory(_container.Kernel);
			ControllerBuilder.Current.SetControllerFactory(controllerFactory);
		}
	}
}