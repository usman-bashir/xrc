using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.IO;
using System.Web;
using xrc.Razor;

// TODO 
// Rivedere questo codice di chiamata a razor. 
// Attualmente chiamo direttamente il ViewEngine così come caricato nel sito web.
// Questo approccio ha alcuni vantaggi perchè offre lo stesso modello di svilupo (compreso intellisense, ...).
// Alternative:
// http://razorengine.codeplex.com/
// http://vibrantcode.com/blog/2010/11/16/hosting-razor-outside-of-aspnet-revised-for-mvc3-rc.html
// L'ultimo link prevede di riscrivere l'engine raor utilizzando solo il parser. In questo modo c'è molta più libertà ma si perdono alcune cose (ad esempio l'intellisense).

namespace xrc.Renderers
{
    public class RazorRenderer : IRenderer
    {
		private IKernel _kernel;
        private Configuration.WorkingPath _workingPath;
        private Modules.IModuleFactory _moduleFactory;
        private Modules.IModuleCatalogService _moduleCatalog;
		public RazorRenderer(IKernel kernel, Configuration.WorkingPath workingPath, Modules.IModuleFactory moduleFactory, Modules.IModuleCatalogService moduleCatalog)
		{
			_kernel = kernel;
            _workingPath = workingPath;
            _moduleCatalog = moduleCatalog;
            _moduleFactory = moduleFactory;
		}

		public string View
		{
			get;
			set;
		}

		public object Model
		{
			get;
			set;
		}

        public void RenderRequest(IContext context)
        {
			if (string.IsNullOrWhiteSpace(View))
				throw new ArgumentNullException("View");

            ViewContext viewContext = new ViewContext();
            viewContext.ViewData = new ViewDataDictionary(Model);
            viewContext.RouteData.Values.Add("controller", "RazorRenderer");
            viewContext.RequestContext = new XrcRequestContext(context);
            viewContext.HttpContext = viewContext.RequestContext.HttpContext;

            LoadParameters(context, viewContext);

            LoadContextVariables(context, viewContext);

            var result = ViewEngines.Engines.FindPartialView(viewContext, GetViewFullName(context));

			if (result.View == null)
				throw new ApplicationException(string.Format("Razor view '{0}' not found.", View));

            var viewEngine = result.ViewEngine;
            var view = result.View;

            try
            {
                view.Render(viewContext, context.Response.Output);
            }
            finally
            {
                viewEngine.ReleaseView(viewContext, view);
            }
        }

        void LoadParameters(IContext context, ViewContext viewContext)
        {
            foreach (var param in context.Parameters)
                viewContext.ViewData.Add(param.Name, param.Value);
        }

        void LoadContextVariables(IContext context, ViewContext viewContext)
        {
            viewContext.ViewData[Razor.ViewDataConstant.VIEWDATA_XRCCONTEXT] = context;
            viewContext.ViewData[Razor.ViewDataConstant.VIEWDATA_IMODULEFACTORY] = _moduleFactory;
            viewContext.ViewData[Razor.ViewDataConstant.VIEWDATA_IMODULECATALOGSERVICE] = _moduleCatalog;
        }

        /// <summary>
        /// Returns a View name in the razor format, starting from the web site path 
        /// </summary>
        string GetViewFullName(IContext context)
        {
            var viewPath = new Uri(context.GetAbsoluteUrl(View));
            var appPath = new Uri(context.GetAbsoluteUrl("~"));
            var relative = viewPath.MakeRelativeUriEx(appPath);
            return UriExtensions.Combine(_workingPath.VirtualPath, relative.ToString());
        }
	}
}


// A custom razor page that can be used as a base class for razor page.
//  The problem is that currently I cannot find an easy method to IoC with WebViewPage so I use a dynamic proxy that create the modules and release it.
//  I load all the modules and parameters defined in the page (using the @functions keyword).
namespace xrc.Razor
{
    public class ViewDataConstant
    {
        public const string VIEWDATA_XRCCONTEXT = "_XrcContext";
        public const string VIEWDATA_IMODULEFACTORY = "_IModuleFactory";
        public const string VIEWDATA_IMODULECATALOGSERVICE = "_IModuleCatalogService";
    }

    public abstract class XrcWebViewPage<TModel> : WebViewPage<TModel>
    {
        public XrcWebViewPage()
        {
        }

        private IContext _xrcContext;
        private Modules.IModuleFactory _moduleFactory;
        private Modules.IModuleCatalogService _moduleCatalog;
        private static readonly Castle.DynamicProxy.ProxyGenerator _generator = new Castle.DynamicProxy.ProxyGenerator();

        public override void InitHelpers()
        {
            base.InitHelpers();

            _xrcContext = (IContext)ViewData[ViewDataConstant.VIEWDATA_XRCCONTEXT];
            _moduleFactory = (Modules.IModuleFactory)ViewData[ViewDataConstant.VIEWDATA_IMODULEFACTORY];
            _moduleCatalog = (Modules.IModuleCatalogService)ViewData[ViewDataConstant.VIEWDATA_IMODULECATALOGSERVICE];

            LoadModulesAndParameters();
        }

        private void LoadModulesAndParameters()
        {
            foreach (var p in GetType().GetProperties())
            {
                if (typeof(Modules.IModule).IsAssignableFrom(p.PropertyType))
                {
                    var component = _moduleCatalog.Get(p.Name);
                    if (p.PropertyType.IsAssignableFrom(component.Type))
                    {
                        var interceptor = new ModuleInterceptor(component, _xrcContext, _moduleFactory);
                        p.SetValue(this, _generator.CreateInterfaceProxyWithoutTarget(p.PropertyType, interceptor), null);
                    }
                }
                else
                {
                    ContextParameter parameter;
                    if (_xrcContext.Parameters.TryGetValue(p.Name, out parameter) &&
                                    p.PropertyType.IsAssignableFrom(parameter.Type))
                        p.SetValue(this, parameter.Value, null);
                }
            }
        }
    }

    /// <summary>
    /// Interceptor to invoke a module inside a page. This allow to create it using the factory and release it.
    /// Note: when MVC will correctly implement dependency injection for razor web page this class can be removed.
    /// </summary>
    class ModuleInterceptor : Castle.DynamicProxy.IInterceptor
    {
        private ComponentDefinition _component;
        private IContext _context;
        private Modules.IModuleFactory _moduleFactory;
        public ModuleInterceptor(ComponentDefinition component, IContext context, Modules.IModuleFactory moduleFactory)
        {
            _component = component;
            _context = context;
            _moduleFactory = moduleFactory;
        }

        public void Intercept(Castle.DynamicProxy.IInvocation invocation)
        {
            Modules.IModule module = _moduleFactory.Get(_component, _context);
            try
            {
                invocation.ReturnValue = invocation.Method.Invoke(module, invocation.Arguments);
            }
            finally
            {
                _moduleFactory.Release(module);
            }
        }
    }
}