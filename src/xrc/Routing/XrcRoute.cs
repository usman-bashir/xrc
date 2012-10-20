using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace xrc.Routing
{
	public class XrcRoute : RouteBase
	{
		private XrcRouteHandler _routeHandler = new XrcRouteHandler();

		public override RouteData GetRouteData(System.Web.HttpContextBase httpContext)
		{
			if (Kernel.Current == null)
				throw new ApplicationException("Kernel not initialized.");

			// TODO How to remove this static call??
			if (Kernel.Current.Match(httpContext))
			{
				var routeData = new RouteData(this, _routeHandler);
				routeData.DataTokens["context"] = context;
				return routeData;
			}
			else
				return null;
		}

		public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
		{
			return null;
		}
	}
}
