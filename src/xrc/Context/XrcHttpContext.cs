using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace xrc
{
    // TODO Verificare meglio implementazione di queste classi

    public class XrcHttpContext : HttpContextBase
    {
        private IContext _context;
        private Dictionary<object, object> _items = new Dictionary<object, object>();

		// TODO Evitare di creare una cache ogni volta?
        private System.Web.Caching.Cache _cache = new System.Web.Caching.Cache();

        public XrcHttpContext(IContext context)
        {
            _context = context;
        }

        public override HttpRequestBase Request
        {
            get
            {
                return _context.Request;
            }
        }

        public override HttpResponseBase Response
        {
            get
            {
                return _context.Response;
            }
        }

        public override System.Collections.IDictionary Items
        {
            get
            {
                return _items;
            }
        }

        public override System.Web.Caching.Cache Cache
        {
            get
            {
                return _cache;
            }
        }
    }
}
