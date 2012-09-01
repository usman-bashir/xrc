using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace xrc.Routing
{
	public class XrcRouteHandler : IRouteHandler
	{
		public System.Web.IHttpHandler GetHttpHandler(RequestContext requestContext)
		{
			IContext context = (IContext)requestContext.RouteData.DataTokens["context"];
			if (context == null)
				throw new XrcException("Context not set");

			return new XrcHttpHandler(context);
		}
	}
}
