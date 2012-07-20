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
        private IRendererFactory _rendererFactory;
        private IScriptService _scriptService;

        public Kernel(IRendererFactory rendererFactory,
                    IMashupParserService parser,
                    ISiteConfigurationProviderService siteConfigurationProvider,
                    IMashupLocatorService fileLocator,
                    IScriptService scriptService)
        {
            _rendererFactory = rendererFactory;
            _parser = parser;
            _siteConfigurationProvider = siteConfigurationProvider;
            _fileLocator = fileLocator;
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

        #region Processing pipeline
        protected virtual void BeginRequest(IContext context)
        {
        }

        protected virtual void EndRequest(IContext context)
        {
        }

        public void RenderRequest(IContext context)
        {
			// TODO se lo user chiede http://localhost:8082/demowebsite/widgets
			// e widgets è una directory, devo fare il redirect su http://localhost:8082/demowebsite/widgets/ (con slash finale?)
            // inoltre fare sempre un redirect con la request lower case (vedere se esiste qualcosa di built-in)

            //Process pipeline

            BeginRequest(context);

            LoadConfiguration(context);

            if (!LocateXrcFile(context))
            {
                ProcessRequestNotFound(context);
                return;
            }

            LoadXrcDefinition(context);

            LoadParameters(context);

            var action = context.Page.Actions[context.Request.HttpMethod];
            if (action == null)
            {
                ProcessRequestNotFound(context);
                return;
            }

            if (!string.IsNullOrWhiteSpace(action.Parent))
                RenderParent(context, action);
            else
            {
                foreach (var renderer in action.Renderers)
                    RunRenderer(context, action, renderer);
            }

            EndRequest(context);
        }

        private void RenderParent(IContext context, MashupAction action)
        {
			HttpResponseBase currentResponse = context.Response;

            // If a parent is defined call first it using the current response 
            // and defining a RenderSlot event that output the current slot inline.
            // The event will be called from the parent action by using Cms.Slot().
            // Parameters will be also copied from slot to parent.

            Uri parentUri = new Uri(context.GetAbsoluteUrl(action.Parent));
            Context parentContext = new Context(new XrcRequest(parentUri), currentResponse);
            foreach (var item in context.Parameters)
                parentContext.Parameters.Add(item.Key, item.Value);

            parentContext.SlotCallback = (s, e) =>
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        using (XrcResponse response = new XrcResponse(stream))
                        {
                            context.Response = response;

                            RendererDefinition rendererDefinition = action.Renderers[e.Name];
                            if (rendererDefinition != null)
                                RunRenderer(context, action, rendererDefinition);
                        }

                        // TODO Come gestire i casi di errore?

                        stream.Flush();
                        stream.Seek(0, SeekOrigin.Begin);

                        using (StreamReader reader = new StreamReader(stream))
                        {
                            e.Content = reader.ReadToEnd();
                        }
                    }
                };

            RenderRequest(parentContext);
        }

        private static void ProcessRequestNotFound(IContext context)
        {
            context.Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
            context.Response.StatusDescription = string.Format("Resource '{0}' not found.", context.Request.Url);
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

        private void LoadParameters(IContext context)
        {
            // Set context parameters
            // the order of the imported parameters define the parameters priority (that last overwrite)

            foreach (var item in context.Configuration.Parameters)
                context.Parameters[item.Key] = item.Value;
            foreach (var item in context.File.UrlSegmentsParameters)
                context.Parameters[item.Key] = item.Value;
            foreach (var item in context.Page.PageParameters)
                context.Parameters[item.Key] = item.Value;
            foreach (var key in context.Request.QueryString.AllKeys)
                context.Parameters[key] = context.Request.QueryString[key];
        }

        private void LoadXrcDefinition(IContext context)
        {
            context.Page = _parser.Parse(context.File.FullPath);
        }

        private void RunRenderer(IContext context, MashupAction action, RendererDefinition rendererDefinition)
        {
            IRenderer renderer = _rendererFactory.Get(rendererDefinition.Component, context);
            try
            {
                foreach (var property in rendererDefinition.Properties)
                {
                    object value;
                    if (property.Expression != null)
                        value = _scriptService.Eval(property.Expression, context);
                    else
                        value = property.Value;

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
