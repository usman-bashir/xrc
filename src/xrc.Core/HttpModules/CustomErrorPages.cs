using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace xrc.HttpModules
{
	// TODO This module should be responsable for logging?
	// Can other module (like elmah) added when using this module? (maybe the ClearError completely clear the errors?)
	// We must add some configuration to handle different httpStatus with different pages (or we can use a single page parametrized).
	// We need to pass parameter to error page

	public class CustomErrorPages : IHttpModule
	{
		public void Init(HttpApplication httpApplication)
		{
			httpApplication.Error += Application_Error;
		}

		void Application_Error(object sender, EventArgs e)
		{
			var exception = Server.GetLastError();

			int httpStatus;

			var httpException = exception as HttpException;
			if (httpException != null)
				httpStatus = httpException.GetHttpCode();
			else
				httpStatus = 500;

			var url = "~/error";

			Response.Clear();
			Response.TrySkipIisCustomErrors = true;
			Server.ClearError();

			var xrcRequest = new xrc.XrcRequest(new xrc.XrcUrl(url), "GET", new HttpRequestWrapper(Context.Request));
			var xrcResponse = new xrc.XrcResponse(new HttpResponseWrapper(Context.Response));
			var context = new xrc.Context(xrcRequest, xrcResponse);

			xrc.Kernel.Current.ProcessRequest(context, new xrc.Sites.SiteConfiguration("default", "."));

			Response.StatusCode = httpStatus;
		}

		public void Dispose() { }

		private HttpResponse Response
		{
			get { return Context.Response; }
		}

		private HttpServerUtility Server
		{
			get { return Context.Server; }
		}

		private HttpContext Context
		{
			get { return HttpContext.Current; }
		}
	}

}
