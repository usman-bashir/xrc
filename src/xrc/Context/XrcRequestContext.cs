using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace xrc
{
    // TODO Verificare meglio implementazione di queste classi

    public class XrcRequestContext : RequestContext
    {
        public XrcRequestContext(IContext context)
            : base(new XrcHttpContext(context), new RouteData()) // TODO Valutare cosa passare a route data
        {
        }

        public XrcRequestContext(HttpContextBase httpContext)
            : base(httpContext, new RouteData()) // TODO Valutare cosa passare a route data
        {
        }
    }
}
