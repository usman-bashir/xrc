using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Pages;
using xrc.Configuration;
using System.IO;
using System.Reflection;
using xrc.Views;
using System.Web;
using xrc.Script;
using xrc.Sites;
using xrc.Modules;
using xrc.Pages.Providers;
using xrc.Pages.Script;

namespace xrc
{
	// TODO Rivedere classe kernel, forse rimuovere e sostituire con i singoli servizi

    public class Kernel : IKernel
    {
		IPageProviderService _pageProvider;
        IViewFactory _viewFactory;
        IModuleFactory _moduleFactory;
		IPageScriptService _scriptService;

        public Kernel(IPageProviderService pageProvider,
                    IViewFactory viewFactory,
                    IModuleFactory moduleFactory,
					IPageScriptService scriptService)
        {
			_pageProvider = pageProvider;
            _viewFactory = viewFactory;
            _moduleFactory = moduleFactory;
			_scriptService = scriptService;
        }

        // TODO Check if it is possible to remove this static reference
        #region Static
        private static IKernel _current;
        public static void Init(IKernel kernel)
        {
            _current = kernel;
        }
        public static IKernel Current
        {
            get { return _current; }
        }
        #endregion

		public void Init()
		{
			Kernel.Init(this);
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
                        RunView(context, action, view, modules);
                }
            }
            finally
            {
                UnloadModules(modules);
            }
        }

        private void RenderParent(IContext context, PageAction action, Dictionary<string, Modules.IModule> modules)
        {
			HttpResponseBase currentResponse = context.Response;

            // If a parent is defined call first it using the current response 
            // and defining a RenderSlot event that output the current slot inline.
            // The event will be called from the parent action by using Cms.Slot().
            // Parameters will be also copied from slot to parent.

            Uri parentUri = context.Page.GetContentAbsoluteUrl(action.Parent);
            Context parentContext = new Context(new XrcRequest(parentUri, parentRequest:context.Request), currentResponse);
			parentContext.CallerContext = context;
            foreach (var item in context.Parameters)
                parentContext.Parameters.Add(new ContextParameter(item.Name, item.Type, item.Value));

            parentContext.SlotCallback = (s, e) =>
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        using (XrcResponse response = new XrcResponse(stream, parentResponse:currentResponse))
                        {
                            context.Response = response;

                            ViewDefinition viewDefinition = action.Views[e.Name];
                            if (viewDefinition != null)
                                RunView(context, action, viewDefinition, modules);
                            else
                                throw new ApplicationException(string.Format("Slot '{0}' not found.", e.Name));
                        }

                        stream.Flush();
                        stream.Seek(0, SeekOrigin.Begin);

                        using (StreamReader reader = new StreamReader(stream))
                        {
                            e.Content = reader.ReadToEnd();
                        }
                    }
                };

            ProcessRequest(parentContext);
            parentContext.CheckError();
        }

        private static void ProcessRequestNotFound(IContext context)
        {
            context.Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
            context.Response.StatusDescription = string.Format("Resource '{0}' not found.", context.Request.Url);
            context.Exception = new ResourceNotFoundException(context.Request.Url.ToString());
        }

        private void ProcessPermanentRedirect(IContext context, Uri canonicalUrl)
        {
            context.Response.RedirectPermanent(canonicalUrl.ToString());
			context.Exception = new MovedPermanentlyException(context.Request.Url.ToString(), canonicalUrl.ToString());
        }

        private Dictionary<string, Modules.IModule> LoadModules(IContext context)
        {
            Dictionary<string, Modules.IModule> modules = new Dictionary<string,Modules.IModule>();
            foreach (var m in context.Page.Modules)
                modules.Add(m.Name, _moduleFactory.Get(m.Component, context));

            return modules;
        }

        private void UnloadModules(Dictionary<string, Modules.IModule> modules)
        {
            foreach (var m in modules)
                _moduleFactory.Release(m.Value);
        }

        private void LoadParameters(IContext context, Dictionary<string, Modules.IModule> modules)
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

        private void RunView(IContext context, PageAction action, ViewDefinition viewDefinition, Dictionary<string, Modules.IModule> modules)
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
