using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using Castle.MicroKernel.Registration;

namespace DemoWebSite
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

		//// Block direct browsing of the xrc root folder
		//protected void Application_BeginRequest()
		//{
		//    var urlRequest = HttpContext.Current.Request.Url;
		//    if (urlRequest.ToString().Contains("root"))
		//    {
		//        HttpContext.Current.Response.StatusCode = 403;
		//        CompleteRequest();
		//    }
		//}

        private void BootstrapContainer()
        {
            _container = new WindsorContainer();

            // register xrc
			var xrcSection = xrc.Configuration.XrcSection.GetSection();
			_container.Install(new xrc.IoC.Windsor.XrcDefaultInstaller(xrcSection));

            // Register demo web site modules
            _container.Register(Classes.FromAssemblyContaining<TwitterModule>()
                                .BasedOn<xrc.Modules.IModule>()
                                .WithServiceSelf()
                                .WithServiceDefaultInterfaces()
								.LifestyleTransient());

            // Windsor MVC integration
            _container.Install(Castle.Windsor.Installer.FromAssembly.This());
            var controllerFactory = new Controllers.WindsorControllerFactory(_container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }

		private void BootstrapXrc(RouteCollection routes)
		{
			xrc.IKernel kernel = _container.Resolve<xrc.IKernel>();
			xrc.Kernel.Init(kernel);

			routes.Add("xrc", new xrc.Routing.XrcRoute());
		}

        private void RegisterRoutes(RouteCollection routes)
		{
			// Standard mvc route
			routes.MapRoute(
				 "Default", // Route name
				 "{controller}/{action}/{id}", // URL with parameters
				 new { controller = "Home", action = "Index", id = UrlParameter.Optional }); // Parameter defaults
		}
    }
}