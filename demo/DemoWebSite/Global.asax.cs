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
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			RegisterRoutes(RouteTable.Routes);
        }

        private void RegisterRoutes(RouteCollection routes)
		{
			routes.MapRoute(
				 "Default", // Route name
				 "{controller}/{action}/{id}", // URL with parameters
				 new { controller = "Home", action = "Index", id = UrlParameter.Optional }); // Parameter defaults
		}
    }
}