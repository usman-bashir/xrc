using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace xrc.Routing
{
	public class XrcRouteHandler : IRouteHandler
	{
		readonly XrcHttpHandler _xrcHttpHandler = new XrcHttpHandler();
		public System.Web.IHttpHandler GetHttpHandler(RequestContext requestContext)
		{
			return _xrcHttpHandler;
		}
	}
}
