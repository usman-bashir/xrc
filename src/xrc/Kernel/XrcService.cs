using System;
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
		readonly Configuration.IRootPathConfig _rootPath;

		public XrcService(IPageProviderService pageProvider,
					IViewFactory viewFactory,
					IModuleFactory moduleFactory,
					IPageScriptService scriptService,
					Configuration.IRootPathConfig rootPath)
        {
			_rootPath = rootPath;
			_pageProvider = pageProvider;
			_viewFactory = viewFactory;
			_moduleFactory = moduleFactory;
			_scriptService = scriptService;
        }

		public System.Web.Mvc.ContentResult Page(XrcUrl url, Sites.ISiteConfiguration siteConfiguration, object parameters = null, IContext callerContext = null)
        {
			try
			{
				var contentResult = new System.Web.Mvc.ContentResult();

				var parentRequest = callerContext == null ? null : callerContext.Request;
				var parentResponse = callerContext == null ? null : callerContext.Response;

				using (var stream = new MemoryStream())
				{
					var request = new XrcRequest(url, parentRequest: parentRequest);
					var response = new XrcResponse(stream, parentResponse: parentResponse);
					var context = new Context(request, response);

					context.CallerContext = callerContext;
					AddParameters(context, parameters);

					ProcessRequest(context, siteConfiguration);

					context.CheckResponse();

					contentResult.ContentEncoding = response.ContentEncoding;
					contentResult.ContentType = response.ContentType;

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
				throw new PageException(url.AppRelaviteUrl, ex);
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

		public bool Match(XrcUrl url)
		{
			return _pageProvider.PageExists(url);
		}

		public void ProcessRequest(IContext context, Sites.ISiteConfiguration siteConfiguration)
		{
			context.Page = _pageProvider.GetPage(context.Request.XrcUrl, siteConfiguration);
			if (context.Page == null)
			{
				ProcessRequestNotFound(context);
				return;
			}

			// Why to redirect to canonical Url to have always the same url (for caching) and for better url architecture
			if (!IsCanonicalUrl(context.Page, context.Request.Url))
			{
				UriBuilder redirectUrl = new UriBuilder(_rootPath.AppRelativeUrlToRelativeUrl(context.Page.VirtualPath));
				redirectUrl.Query = context.Request.Url.Query;
				ProcessPermanentRedirect(context, redirectUrl.Uri);
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

				if (!string.IsNullOrWhiteSpace(action.Layout))
					RenderLayout(context, action, modules);
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

		private bool IsCanonicalUrl(IPage page, Uri requestUri)
		{
			var canonicalPath = _rootPath.AppRelativeUrlToRelativeUrl(page.VirtualPath).GetPath();
			var requestedPath = requestUri.GetPath();

			return string.Equals(canonicalPath, requestedPath, StringComparison.Ordinal);
		}

		private void RenderLayout(IContext childContext, PageAction childAction, Dictionary<string, object> childModules)
		{
			XrcResponse currentResponse = childContext.Response;

			// If a parent is defined call first it using the current response 
			// and defining a SlotCallback event that output the current slot inline.
			// The event will be called from the layout action by using Cms.Slot().
			// Parameters will be also copied from slot to layout.

			string layoutPage = childAction.Layout.ToLower();
			string appRelativeLayoutPage = childContext.Page.GetContentVirtualUrl(layoutPage);
			Context layoutContext = new Context(new XrcRequest(new XrcUrl(appRelativeLayoutPage), parentRequest: childContext.Request), currentResponse);
			layoutContext.CallerContext = childContext;
			foreach (var item in childContext.Parameters)
				layoutContext.Parameters.Add(new ContextParameter(item.Name, item.Type, item.Value));

			layoutContext.SlotCallback = (s, e) =>
			{
				var childResult = new System.Web.Mvc.ContentResult();
				using (MemoryStream stream = new MemoryStream())
				{
					XrcResponse response = new XrcResponse(stream, parentResponse: currentResponse);
					childContext.Response = response;

					ViewDefinition viewDefinition = childAction.Views[e.Name];
					if (viewDefinition != null)
						ExecuteView(childContext, childAction, viewDefinition, childModules);
					else
						throw new ApplicationException(string.Format("Slot '{0}' not found.", e.Name));

					childResult.ContentEncoding = response.ContentEncoding;
					childResult.ContentType = response.ContentType;

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
				ProcessRequest(layoutContext, childContext.Page.SiteConfiguration);
				layoutContext.CheckResponse();
			}
			catch (Exception ex)
			{
				// Set a generic status code. We don't want to expose directly parent StatusCode like redirect 
				//  otherwise the client is redirected to a wrong page (the parent page).
				currentResponse.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
				throw new PageException(appRelativeLayoutPage, ex);
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

			// Set the parameters only when not already present, so save the list of inherited parameters
			string[] inheritdKeys = context.Parameters.Select(p => p.Name).ToArray();

			foreach (var item in context.Page.SiteConfiguration.Parameters)
			{
				if (!inheritdKeys.Contains(item.Key, StringComparer.OrdinalIgnoreCase))
					context.Parameters[item.Key] = new ContextParameter(item.Key, typeof(string), item.Value);
			}

			foreach (var item in context.Page.UrlSegmentsParameters)
			{
				if (!inheritdKeys.Contains(item.Key, StringComparer.OrdinalIgnoreCase))
					context.Parameters[item.Key] = new ContextParameter(item.Key, typeof(string), item.Value);
			}

			// Page.Parameters override any other values except Request if the allowRequestOverride is true.
			// Page.Parameters type is used for conversion.
			foreach (var pageParam in context.Page.PageParameters)
			{
				if (inheritdKeys.Contains(pageParam.Name, StringComparer.OrdinalIgnoreCase))
					continue;

				string requestValue;
				// Read parameters from the request
				if (pageParam.AllowRequestOverride && (requestValue = context.Request[pageParam.Name]) != null)
				{
					context.Parameters[pageParam.Name] = new ContextParameter(pageParam.Name, pageParam.Value.ValueType,
															ConvertEx.ChangeType(requestValue, pageParam.Value.ValueType, System.Globalization.CultureInfo.InvariantCulture));
				}
				// Read parameters already specified in the context (for example by the parent)
				else if (pageParam.Value.Expression == null && pageParam.Value.Value == null)
				{
					ContextParameter currentValue;
					if (context.Parameters.TryGetValue(pageParam.Name, out currentValue))
						context.Parameters[pageParam.Name] = new ContextParameter(pageParam.Name, pageParam.Value.ValueType,
																ConvertEx.ChangeType(currentValue.Value, pageParam.Value.ValueType, System.Globalization.CultureInfo.InvariantCulture));
					else
						throw new ApplicationException(string.Format("Parameter '{0}' not defined.", pageParam.Name));
				}
				else // Read parameters specified in the page
				{
					object value = _scriptService.Eval(pageParam.Value, modules, context.Parameters);
					context.Parameters[pageParam.Name] = new ContextParameter(pageParam.Name, pageParam.Value.ValueType, value);
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
