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
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext httpContext)
        {
			if (Kernel.Current == null)
                throw new ApplicationException("Kernel not initialized.");

			// TODO How to remove this static call??
			Kernel.Current.ProcessRequest(new HttpContextWrapper(httpContext));
        }
    }
}
