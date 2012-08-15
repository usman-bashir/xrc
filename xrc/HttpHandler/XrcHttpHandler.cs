using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace xrc
{
	// TODO Check if it's better and possible to use IHttpAsyncHandler or the new async model

    public class XrcHttpHandler : IHttpHandler 
    {
		private IContext _context;

		public XrcHttpHandler(IContext context)
		{
			_context = context;
		}

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext httpContext)
        {
			if (Kernel.Current == null)
                throw new ApplicationException("Kernel not initialized.");

			// TODO Should I need to check if httpContext request match _context request?

			//Context context = new Context(new HttpRequestWrapper(httpContext.Request),
			//                            new HttpResponseWrapper(httpContext.Response));

			// TODO How to remove this static call??
			Kernel.Current.ProcessRequest(_context);
        }
    }
}
