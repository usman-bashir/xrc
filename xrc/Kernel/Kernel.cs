using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.SiteManager;
using xrc.Configuration;
using System.IO;
using System.Reflection;
using xrc.Renderers;
using System.Web;
using xrc.Script;

namespace xrc
{
    public class Kernel : IKernel
    {
        private IMashupParserService _parser;
        private ISiteConfigurationProviderService _siteConfigurationProvider;
        private IMashupLocatorService _fileLocator;
        private IMashupScriptService _scriptService;
        private IRendererFactory _rendererFactory;
        private Modules.IModuleFactory _moduleFactory;

        public Kernel(IMashupParserService parser,
                    ISiteConfigurationProviderService siteConfigurationProvider,
                    IMashupLocatorService fileLocator,
                    IRendererFactory rendererFactory,
                    IMashupScriptService scriptService,
                    Modules.IModuleFactory moduleFactory)
        {
            _parser = parser;
            _siteConfigurationProvider = siteConfigurationProvider;
            _fileLocator = fileLocator;
            _rendererFactory = rendererFactory;
            _scriptService = scriptService;
            _moduleFactory = moduleFactory;
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

        #region Processing pipeline
        public void RenderRequest(IContext context)
        {
            //Process pipeline

            LoadConfiguration(context);

            if (!LocateXrcFile(context))
            {
                ProcessRequestNotFound(context);
                return;
            }

            string canonicalUrl;
            if (IsCanonicalUrl(context, out canonicalUrl))
            {
                ProcessPermanentRedirect(context, canonicalUrl);
                return;
            }

            LoadXrcDefinition(context);

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
                    foreach (var renderer in action.Renderers)
                        RunRenderer(context, action, renderer, modules);
                }
            }
            finally
            {
                UnloadModules(modules);
            }
        }

        private bool IsCanonicalUrl(IContext context, out string canonicalUrl)
        {
            canonicalUrl = context.GetAbsoluteUrl(context.File.CanonicalUrl);

            return canonicalUrl != context.Request.Url.GetLeftPart(UriPartial.Path);
        }

        private void RenderParent(IContext context, MashupAction action, Dictionary<string, Modules.IModule> modules)
        {
			HttpResponseBase currentResponse = context.Response;

            // If a parent is defined call first it using the current response 
            // and defining a RenderSlot event that output the current slot inline.
            // The event will be called from the parent action by using Cms.Slot().
            // Parameters will be also copied from slot to parent.

            Uri parentUri = new Uri(context.GetAbsoluteUrl(action.Parent));
            Context parentContext = new Context(new XrcRequest(parentUri, parentRequest:context.Request), currentResponse);
            foreach (var item in context.Parameters)
                parentContext.Parameters.Add(new ContextParameter(item.Name, item.Type, item.Value));

            parentContext.SlotCallback = (s, e) =>
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        using (XrcResponse response = new XrcResponse(stream, parentResponse:currentResponse))
                        {
                            context.Response = response;

                            RendererDefinition rendererDefinition = action.Renderers[e.Name];
                            if (rendererDefinition != null)
                                RunRenderer(context, action, rendererDefinition, modules);
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

            RenderRequest(parentContext);
            parentContext.CheckError();
        }

        private static void ProcessRequestNotFound(IContext context)
        {
            context.Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
            context.Response.StatusDescription = string.Format("Resource '{0}' not found.", context.Request.Url);
            context.Exception = new ResourceNotFoundException(context.Request.Url.ToString());
        }

        private void ProcessPermanentRedirect(IContext context, string canonicalUrl)
        {
            context.Response.RedirectPermanent(canonicalUrl);
            context.Exception = new MovedPermanentlyException(context.Request.Url.ToString(), canonicalUrl);
        }

        private void LoadConfiguration(IContext context)
        {
            context.Configuration = _siteConfigurationProvider.GetSiteFromUri(context.Request.Url);
        }

        private bool LocateXrcFile(IContext context)
        {
			context.File = _fileLocator.Locate(context.Configuration.GetRelativeUrl(context.Request.Url));
            if (context.File == null)
                return false;
            else
                return true;
        }

        private void LoadXrcDefinition(IContext context)
        {
            context.Page = _parser.Parse(context.File.FullPath);
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

            foreach (var item in context.Configuration.Parameters)
                context.Parameters[item.Key] = new ContextParameter(item.Key, typeof(string), item.Value);

            foreach (var item in context.File.UrlSegmentsParameters)
                context.Parameters[item.Key] = new ContextParameter(item.Key, typeof(string), item.Value);

            // Page.Parameters override any other values except Request if the allowRequestOverride is true.
            // Page.Parameters type is used for conversion.
            foreach (var item in context.Page.Parameters)
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

        private void RunRenderer(IContext context, MashupAction action, RendererDefinition rendererDefinition, Dictionary<string, Modules.IModule> modules)
        {
            IRenderer renderer = _rendererFactory.Get(rendererDefinition.Component, context);
            try
            {
                foreach (var property in rendererDefinition.Properties)
                {
                    object value = _scriptService.Eval(property.Value, modules, context.Parameters);
                    property.PropertyInfo.SetValue(renderer, value, null);
                }

                renderer.RenderRequest(context);
            }
            finally
            {
                _rendererFactory.Release(renderer);
            }
        }
        #endregion
    }
}
