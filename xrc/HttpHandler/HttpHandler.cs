using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace xrc
{
	// TODO Probably it's better to use IHttpAsyncHandler
	// or the new async model

    public class HttpHandler : IHttpHandler 
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext httpContext)
        {
			if (Kernel.Current == null)
                throw new ApplicationException("Kernel not initialized.");

            Context context = new Context(new HttpRequestWrapper(httpContext.Request),
                                        new HttpResponseWrapper(httpContext.Response));

            Kernel.Current.RenderRequest(context);
        }
    }
}
