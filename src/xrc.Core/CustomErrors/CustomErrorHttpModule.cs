using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace xrc.CustomErrors
{
	// TODO This module should be responsable for logging?
	// Can other module (like elmah) added when using this module? (maybe the ClearError completely clear the errors?)
	// We must add some configuration to handle different httpStatus with different pages (or we can use a single page parametrized).
	// We need to pass parameter to error page

	public class CustomErrorHttpModule : IHttpModule
	{
		readonly Configuration.ICustomErrorsConfig _customErrorConfig;
		public CustomErrorHttpModule(Configuration.ICustomErrorsConfig customErrorConfig)
		{
			_customErrorConfig = customErrorConfig;
		}

		public void Init(HttpApplication httpApplication)
		{
			httpApplication.Error += Application_Error;
		}

		void Application_Error(object sender, EventArgs e)
		{
			var exception = Server.GetLastError();

			var httpStatus = GetHttpStatus(exception);

			var url = GetErrorPage(httpStatus);

			if (string.IsNullOrWhiteSpace(url))
				return;

			ProcessErrorPage(httpStatus, url);
		}

		void ProcessErrorPage(int httpStatus, string url)
		{
			Response.Clear();
			Response.TrySkipIisCustomErrors = true;
			Server.ClearError();

			var xrcRequest = new xrc.XrcRequest(new xrc.XrcUrl(url), "GET", new HttpRequestWrapper(Context.Request));
			var xrcResponse = new xrc.XrcResponse(new HttpResponseWrapper(Context.Response));
			var context = new xrc.Context(xrcRequest, xrcResponse);

			xrc.Kernel.Current.ProcessRequest(context);

			Response.StatusCode = httpStatus;
		}

		string GetErrorPage(int httpStatus)
		{
			var url = _customErrorConfig.GetErrorPage(httpStatus);
			return url;
		}

		int GetHttpStatus(Exception exception)
		{
			int httpStatus;

			var httpException = exception as HttpException;
			if (httpException != null)
				httpStatus = httpException.GetHttpCode();
			else
				httpStatus = 500;
			return httpStatus;
		}

		public void Dispose() { }

		HttpResponse Response
		{
			get { return Context.Response; }
		}

		HttpServerUtility Server
		{
			get { return Context.Server; }
		}

		HttpContext Context
		{
			get { return HttpContext.Current; }
		}
	}

}
