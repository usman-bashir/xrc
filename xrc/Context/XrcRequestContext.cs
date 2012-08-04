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
        private IContext _context;

        public XrcRequestContext(IContext context)
            : base(new XrcHttpContext(context), new RouteData()) // TODO Valutare cosa passare a route data
        {
            _context = context;
        }
    }
}
