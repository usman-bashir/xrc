﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Xml.XPath;
using xrc.Pages.Providers;
using xrc.Views;
using xrc.Modules;
using xrc.Pages.Script;
using xrc.Pages;

namespace xrc
{
	// TODO Rivedere classe XrcService

    public class XrcService : IXrcService
    {
		readonly IPageProviderService _pageProvider;
		readonly IViewFactory _viewFactory;
		readonly IModuleFactory _moduleFactory;
		readonly IPageScriptService _scriptService;

		public XrcService(IPageProviderService pageProvider,
					IViewFactory viewFactory,
					IModuleFactory moduleFactory,
					IPageScriptService scriptService)
        {
			_pageProvider = pageProvider;
			_viewFactory = viewFactory;
			_moduleFactory = moduleFactory;
			_scriptService = scriptService;
        }

        public System.Web.Mvc.ContentResult Page(Uri url, object parameters = null, IContext callerContext = null)
        {
			try
			{
				var contentResult = new System.Web.Mvc.ContentResult();

				var parentRequest = callerContext == null ? null : callerContext.Request;
				var parentResponse = callerContext == null ? null : callerContext.Response;

				using (MemoryStream stream = new MemoryStream())
				{
					XrcRequest request = new XrcRequest(url.ToLower(), parentRequest: parentRequest);

					using (XrcResponse response = new XrcResponse(stream, parentResponse: parentResponse))
					{
						Context context = new Context(request, response);
						context.CallerContext = callerContext;
						AddParameters(context, parameters);

						ProcessRequest(context);

						context.CheckResponse();

						contentResult.ContentEncoding = response.ContentEncoding;
						contentResult.ContentType = response.ContentType;
					}

					stream.Flush();
					stream.Seek(0, SeekOrigin.Begin);

					using (StreamReader reader = new StreamReader(stream))
					{
						contentResult.Content = reader.ReadToEnd();
					}
				}

				return contentResult;
			}
			catch (Exception ex)
			{
				throw new PageException(url, ex);
			}
        }

        private void AddParameters(IContext context, object parameters)
        {
            if (parameters == null)
                return;

            // TODO Valutare se è possibile accettare i parametri in altro formato. Ma sembra che da xslt gli unici formati sono XPathNavigator o tipi primitivi (vedi http://msdn.microsoft.com/en-us/library/533texsx(VS.71).aspx ).
            // TODO Gestire anche altri tipi quando il metodo è chiamato da altri engine (es. razor)
            if (parameters is System.Xml.XPath.XPathNavigator)
            {
                System.Xml.XPath.XPathNavigator parametersNode = parameters as System.Xml.XPath.XPathNavigator;
                foreach (System.Xml.XPath.XPathNavigator nodeParameter in parametersNode.SelectChildren(XPathNodeType.Element))
                {
                    context.Parameters.Add(new ContextParameter(nodeParameter.Name, typeof(string), nodeParameter.Value));
                }
            }
            else
            {
                // Read all the properties (used when parameters is an anonymous type)
                foreach (var p in parameters.GetType().GetProperties())
                {
                    context.Parameters.Add(new ContextParameter(p.Name, p.PropertyType, p.GetValue(parameters, null)));
                }
            }
            //else
            //    throw new XrcException(string.Format("Parameters type '{0}' not supported.", parameters.GetType()));
        }

		public bool Match(IContext context)
		{
			return _pageProvider.IsDefined(context.Request.Url);
		}

		public void ProcessRequest(IContext context)
		{
			context.Page = _pageProvider.GetPage(context.Request.Url);
			if (context.Page == null)
			{
				ProcessRequestNotFound(context);
				return;
			}

			// TODO Why we need to redirect to canonical Url? Only for caching? I think there is another reason but I don't remeber...
			if (context.Page.CanonicalUrl.ToString() != context.Request.Url.GetLeftPart(UriPartial.Path))
			{
				ProcessPermanentRedirect(context, context.Page.CanonicalUrl);
				return;
			}

			var modules = LoadModules(context);
			try
			{
				LoadParameters(context, modules);

				var action = context.Page.Actions[context.Request.HttpMethod];
				if (action == null)
				{
					ProcessRequestNotFound(context);
					return;
				}

				if (!string.IsNullOrWhiteSpace(action.Parent))
					RenderParent(context, action, modules);
				else
				{
					foreach (var view in action.Views)
						ExecuteView(context, action, view, modules);
				}
			}
			finally
			{
				UnloadModules(modules);
			}
		}

		private void RenderParent(IContext childContext, PageAction childAction, Dictionary<string, object> childModules)
		{
			HttpResponseBase currentResponse = childContext.Response;

			// If a parent is defined call first it using the current response 
			// and defining a SlotCallback event that output the current slot inline.
			// The event will be called from the parent action by using Cms.Slot().
			// Parameters will be also copied from slot to parent.

			Uri parentUrl = childContext.Page.ToAbsoluteUrl(childAction.Parent.ToLower());
			Context parentContext = new Context(new XrcRequest(parentUrl, parentRequest: childContext.Request), currentResponse);
			parentContext.CallerContext = childContext;
			foreach (var item in childContext.Parameters)
				parentContext.Parameters.Add(new ContextParameter(item.Name, item.Type, item.Value));

			parentContext.SlotCallback = (s, e) =>
			{
				var childResult = new System.Web.Mvc.ContentResult();
				using (MemoryStream stream = new MemoryStream())
				{
					using (XrcResponse response = new XrcResponse(stream, parentResponse: currentResponse))
					{
						childContext.Response = response;

						ViewDefinition viewDefinition = childAction.Views[e.Name];
						if (viewDefinition != null)
							ExecuteView(childContext, childAction, viewDefinition, childModules);
						else
							throw new ApplicationException(string.Format("Slot '{0}' not found.", e.Name));

						childResult.ContentEncoding = response.ContentEncoding;
						childResult.ContentType = response.ContentType;
					}

					stream.Flush();
					stream.Seek(0, SeekOrigin.Begin);

					using (StreamReader reader = new StreamReader(stream))
					{
						childResult.Content = reader.ReadToEnd();
					}
				}

				e.Result = childResult;
			};

			try
			{
				ProcessRequest(parentContext);
				parentContext.CheckResponse();
			}
			catch (Exception ex)
			{
				// Set a generic status code. We don't want to expose directly parent StatusCode like redirect 
				//  otherwise the client is redirected to a wrong page (the parent page).
				currentResponse.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
				throw new PageException(parentUrl, ex);
			}
		}

		private static void ProcessRequestNotFound(IContext context)
		{
			context.Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
			context.Response.StatusDescription = string.Format("Resource '{0}' not found.", context.Request.Url);
			context.Exception = new ResourceNotFoundException(context.Request.Url.ToString());
		}

		private void ProcessPermanentRedirect(IContext context, Uri canonicalUrl)
		{
			context.Response.RedirectPermanent(canonicalUrl.ToString(), false);
			context.Exception = new MovedPermanentlyException(context.Request.Url.ToString(), canonicalUrl.ToString());
		}

		private Dictionary<string, object> LoadModules(IContext context)
		{
			var modules = new Dictionary<string, object>();
			foreach (var m in context.Page.Modules)
				modules.Add(m.Name, _moduleFactory.Get(m.Component, context));

			return modules;
		}

		private void UnloadModules(Dictionary<string, object> modules)
		{
			foreach (var m in modules)
				_moduleFactory.Release(m.Value);
		}

		private void LoadParameters(IContext context, Dictionary<string, object> modules)
		{
			// Note: Automatically read only server parameters (Configuration, UrlSegments, Page),
			// user parameters (cookie, query string, post, header, ...) are readed only on request

			foreach (var item in context.Page.SiteConfiguration.Parameters)
				context.Parameters[item.Key] = new ContextParameter(item.Key, typeof(string), item.Value);

			foreach (var item in context.Page.UrlSegmentsParameters)
				context.Parameters[item.Key] = new ContextParameter(item.Key, typeof(string), item.Value);

			// Page.Parameters override any other values except Request if the allowRequestOverride is true.
			// Page.Parameters type is used for conversion.
			foreach (var item in context.Page.PageParameters)
			{
				string requestValue;
				if (item.AllowRequestOverride && (requestValue = context.Request[item.Name]) != null)
				{
					context.Parameters[item.Name] = new ContextParameter(item.Name, item.Value.ValueType,
															ConvertEx.ChangeType(requestValue, item.Value.ValueType, System.Globalization.CultureInfo.InvariantCulture));
				}
				else if (item.Value.Expression == null && item.Value.Value == null)
				{
					ContextParameter currentValue;
					if (context.Parameters.TryGetValue(item.Name, out currentValue))
						context.Parameters[item.Name] = new ContextParameter(item.Name, item.Value.ValueType,
																ConvertEx.ChangeType(currentValue.Value, item.Value.ValueType, System.Globalization.CultureInfo.InvariantCulture));
					else
						throw new ApplicationException(string.Format("Parameter '{0}' not defined.", item.Name));
				}
				else
				{
					object value = _scriptService.Eval(item.Value, modules, context.Parameters);
					context.Parameters[item.Name] = new ContextParameter(item.Name, item.Value.ValueType, value);
				}
			}
		}

		private void ExecuteView(IContext context, PageAction action, ViewDefinition viewDefinition, Dictionary<string, object> modules)
		{
			IView view = _viewFactory.Get(viewDefinition.Component, context);
			try
			{
				foreach (var property in viewDefinition.Properties)
				{
					object value = _scriptService.Eval(property.Value, modules, context.Parameters);
					property.PropertyInfo.SetValue(view, value, null);
				}

				view.Execute(context);
			}
			finally
			{
				_viewFactory.Release(view);
			}
		}
    }
}