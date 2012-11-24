using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace xrc.Routing
{
	public class XrcRoute : RouteBase
	{
		readonly RouteData _routeData;
		public XrcRoute()
		{
			_routeData = new RouteData(this, new XrcRouteHandler());
		}

		public override RouteData GetRouteData(System.Web.HttpContextBase httpContext)
		{
			if (Kernel.Current == null)
				throw new ApplicationException("Kernel not initialized.");

			// TODO How to remove this static call??
			if (Kernel.Current.Match(httpContext))
				return _routeData;
			else
				return null;
		}

		public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
		{
			//TODO esplorare meglio questo metodo, forse potrebbe sostituire la gestione degli urlparameters
			return null;
		}
	}
}
