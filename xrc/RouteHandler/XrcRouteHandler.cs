using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace xrc
{
	public class XrcRouteHandler : IRouteHandler
	{
		public System.Web.IHttpHandler GetHttpHandler(RequestContext requestContext)
		{
			// TODO Valutare come e se fare cache dell'handler
			return new HttpHandler();
		}
	}
}
