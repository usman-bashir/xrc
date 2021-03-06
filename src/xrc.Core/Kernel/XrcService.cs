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
	// TODO Rivedere

    public class XrcService : IXrcService
    {
		readonly IPageProviderService _pageProvider;
		readonly IViewFactory _viewFactory;
		readonly IModuleFactory _moduleFactory;
		readonly IPageScriptService _scriptService;
		readonly Configuration.IHostingConfig _hostingConfig;
		readonly Configuration.IEnvironmentConfig _environmentConfig;

		public XrcService(IPageProviderService pageProvider,
					IViewFactory viewFactory,
					IModuleFactory moduleFactory,
					IPageScriptService scriptService,
					Configuration.IHostingConfig hostingConfig,
					Configuration.IEnvironmentConfig environmentConfig)
        {
			_hostingConfig = hostingConfig;
			_environmentConfig = environmentConfig;
			_pageProvider = pageProvider;
			_viewFactory = viewFactory;
			_moduleFactory = moduleFactory;
			_scriptService = scriptService;
        }

		public StringResult Page(XrcUrl url, object parameters = null, IContext callerContext = null)
        {
			try
			{
				var parentRequest = callerContext == null ? null : callerContext.Request;
				var parentResponse = callerContext == null ? null : callerContext.Response;

				using (var stream = new MemoryStream())
				{
					var request = new XrcRequest(url, parentRequest: parentRequest);
					var response = new XrcResponse(stream, parentResponse: parentResponse);
					var context = new Context(request, response);

					context.CallerContext = callerContext;
					AddParameters(context, parameters);

					ProcessRequest(context);

					context.CheckResponse();

					response.Flush();

					stream.Seek(0, SeekOrigin.Begin);

					using (StreamReader reader = new StreamReader(stream, response.ContentEncoding))
					{
                        return new StringResult(reader.ReadToEnd(), response.ContentEncoding, response.ContentType);
					}
				}
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
        }

		public bool Match(XrcUrl url)
		{
			return _pageProvider.PageExists(url);
		}

		public void ProcessRequest(IContext context)
		{
			context.Page = _pageProvider.GetPage(context.Request.XrcUrl);
			if (context.Page == null)
			{
				ProcessRequestNotFound(context);
				return;
			}

			// Redirect to canonical Url to have always the same url (for caching) and for better url architecture, but only for GET verb
			if (!IsCanonicalUrl(context.Page, context.Request.XrcUrl)
				&& context.Request.HttpMethod == "GET")
			{
				Uri canonicalUrl = GetCanonicalUrl(context);
				ProcessPermanentRedirect(context, canonicalUrl);
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

		private Uri GetCanonicalUrl(IContext context)
		{
			Uri relativeUrl = _hostingConfig.AppRelativeUrlToRelativeUrl(context.Page.PageUrl.ToString());

			// TODO Devo usare un dominio fittizio perchè UriBuilder non supporta relative url. Verificare se c'è un metodo migliore.
			// Probabilmente l'url compreso di query string dovrebbe essere un parametro della Page

			UriBuilder canonicalUrlBuilder = new UriBuilder("http", "dummy", 80, 
															relativeUrl.GetPath(),
															context.Request.Url.Query);

			Uri canonicalUrl = new Uri(canonicalUrlBuilder.Uri.PathAndQuery, UriKind.Relative);
			return canonicalUrl;
		}

		private bool IsCanonicalUrl(IPage page, XrcUrl requestUri)
		{
			var canonicalPath = page.PageUrl.Path;
			var requestedPath = requestUri.Path;

			return string.Equals(canonicalPath, requestedPath, StringComparison.Ordinal);
		}

		private void RenderLayout(IContext childContext, PageAction childAction, Dictionary<string, object> childModules)
		{
			XrcResponse currentResponse = childContext.Response;

			// If a parent is defined call first it using the current response 
			// and defining a SlotCallback event that output the current slot inline.
			// The event will be called from the layout action by using Cms.Slot().
			// Parameters will be also copied from slot to layout.

			XrcUrl layoutPage = childContext.Page.GetPageUrl(childAction.Layout.ToLower());
			Context layoutContext = new Context(new XrcRequest(layoutPage, parentRequest: childContext.Request), currentResponse);
			layoutContext.CallerContext = childContext;
			foreach (var item in childContext.Parameters)
				layoutContext.Parameters.Add(new ContextParameter(item.Name, item.Type, item.Value));

			layoutContext.SlotCallback = (s, e) =>
			{
				var childResult = new StringResult();
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

					response.Flush();

					stream.Seek(0, SeekOrigin.Begin);

                    using (StreamReader reader = new StreamReader(stream, response.ContentEncoding))
					{
						childResult.Content = reader.ReadToEnd();
					}
				}

				e.Result = childResult;
			};

			try
			{
				ProcessRequest(layoutContext);
				layoutContext.CheckResponse();
			}
			catch (Exception ex)
			{
				// Set a generic status code. We don't want to expose directly parent StatusCode like redirect 
				//  otherwise the client is redirected to a wrong page (the parent page).
				currentResponse.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
				throw new PageException(layoutPage.ToString(), ex);
			}
		}

		private static void ProcessRequestNotFound(IContext context)
		{
			string resourceUrl = context.Request.XrcUrl.ToString();

			context.Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
			context.Response.StatusDescription = string.Format("Resource '{0}' not found.", resourceUrl);
			context.Exception = new ResourceNotFoundException(resourceUrl);
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

			foreach (var item in _environmentConfig.Settings)
			{
				if (!inheritdKeys.Contains(item.Key, StringComparer.OrdinalIgnoreCase) && item.Value != null)
					context.Parameters[item.Key] = new ContextParameter(item.Key, item.Value.GetType(), item.Value);
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
			catch (Exception ex)
			{
				if (action.CatchException != null)
					ProcessActionException(context, action, ex);
				else
					throw;
			}
			finally
			{
				_viewFactory.Release(view);
			}
		}

		private void ProcessActionException(IContext context, PageAction action, Exception ex)
		{
			XrcUrl errorPage = context.Page.GetPageUrl(action.CatchException.Url.ToLower());
			var errorContent = Page(errorPage, new { Exception = ex }, context);

			// TODO If there is already some content on the stream? An exception can occurs when some content is already written... Think if this is acceptable.

			context.Response.Write(errorContent.Content);
		}
    }
}
