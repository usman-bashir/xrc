using System.Web.Mvc;
using Castle.Windsor;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

[assembly: WebActivator.PreApplicationStartMethod(
    typeof(DemoWebSite.XrcAppStart), "PreStart")]

namespace DemoWebSite
{
	public class XrcAppStart
	{
		static IWindsorContainer _container;

		public static void PreStart()
		{
			BootstrapContainer();

			BootstrapXrc();

			SetMVCControllerFactory();

			RegisterModules();
		}

		static void BootstrapContainer()
		{
			_container = new WindsorContainer();

			// register xrc
			var xrcSection = xrc.Configuration.XrcSection.GetSection();
			_container.Install(new xrc.IoC.Windsor.XrcDefaultInstaller(xrcSection));

			_container.Install(Castle.Windsor.Installer.FromAssembly.This());
		}

		static void BootstrapXrc()
		{
			xrc.IKernel kernel = _container.Resolve<xrc.IKernel>();
			kernel.Init();
		}

		static void SetMVCControllerFactory()
		{
			var controllerFactory = new Windsor.WindsorControllerFactory(_container.Kernel);
			ControllerBuilder.Current.SetControllerFactory(controllerFactory);
		}

		static void RegisterModules()
		{
			DynamicModuleUtility.RegisterModule(typeof(xrc.CustomErrors.CustomErrorHttpModule)); // .NET 4.0
			// HttpApplication.RegisterModule // .NET 4.5
		}
	}
}