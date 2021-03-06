﻿using System;
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
			if (exception == null)
				return;

			var httpStatus = GetHttpStatus(exception);
			var customErrorUrl = GetErrorPage(httpStatus);

			if (string.IsNullOrWhiteSpace(customErrorUrl))
				return;

			ProcessErrorPage(exception, httpStatus, customErrorUrl);
		}

		void ProcessErrorPage(Exception exception, int httpStatus, string customErrorUrl)
		{
			Response.Clear();
			Server.ClearError();

			Response.StatusCode = httpStatus;
			Response.TrySkipIisCustomErrors = true;

			var xrcRequest = new xrc.XrcRequest(new xrc.XrcUrl(customErrorUrl), "GET", new HttpRequestWrapper(Context.Request));
			var xrcResponse = new xrc.XrcResponse(new HttpResponseWrapper(Context.Response));
			var context = new xrc.Context(xrcRequest, xrcResponse);
			context.Parameters.Add(new ContextParameter("Exception", typeof(Exception), exception));
			context.Parameters.Add(new ContextParameter("HttpStatus", typeof(int), httpStatus));
			context.Parameters.Add(new ContextParameter("ErrorUrl", typeof(Uri), Context.Request.Url));

			xrc.Kernel.Current.ProcessRequest(context);
		}

		string GetErrorPage(int httpStatus)
		{
			var url = _customErrorConfig.GetErrorPage(httpStatus);
			return url;
		}

		int GetHttpStatus(Exception exception)
		{
			var httpException = exception as HttpException;
			if (httpException != null)
				return httpException.GetHttpCode();
			else if (exception.InnerException != null)
				return GetHttpStatus(exception.InnerException);
			else
				return 500;
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
