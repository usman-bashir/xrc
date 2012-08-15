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
// Attualmente chiamo direttamente il Razor ViewEngine così come caricato nel sito web.
// Questo approccio ha alcuni vantaggi perchè offre lo stesso modello di svilupo (compreso intellisense, ...) sulla pagina razor perchè viene invocata dal viewengine di default.
// Alternative:
// http://razorengine.codeplex.com/
// http://vibrantcode.com/blog/2010/11/16/hosting-razor-outside-of-aspnet-revised-for-mvc3-rc.html
// L'ultimo link prevede di riscrivere l'engine razor utilizzando solo il parser. In questo modo c'è molta più libertà ma si perdono alcune cose (ad esempio l'intellisense).

namespace xrc.Views
{
    public class RazorView : IView
    {
		private IKernel _kernel;
        private Configuration.RootPath _workingPath;
        private Modules.IModuleFactory _moduleFactory;
        private Modules.IModuleCatalogService _moduleCatalog;
		public RazorView(IKernel kernel, Configuration.RootPath workingPath, Modules.IModuleFactory moduleFactory, Modules.IModuleCatalogService moduleCatalog)
		{
			_kernel = kernel;
            _workingPath = workingPath;
            _moduleCatalog = moduleCatalog;
            _moduleFactory = moduleFactory;
		}

		public string ViewFile
		{
			get;
			set;
		}

		public object Model
		{
			get;
			set;
		}

        public void Execute(IContext context)
        {
			if (string.IsNullOrWhiteSpace(ViewFile))
				throw new ArgumentNullException("View");

            ViewContext viewContext = new ViewContext();
            viewContext.ViewData = new ViewDataDictionary(Model);
            viewContext.RouteData.Values.Add("controller", "RazorView");
            if (HttpContext.Current == null)
                viewContext.RequestContext = new XrcRequestContext(context);
            else
                viewContext.RequestContext = new XrcRequestContext(new HttpContextWrapper(HttpContext.Current)); // TODO Sembra che questo codice in release su IIS non funzionava (anche su azure), era qualcosa legato alla cache (DefaultViewLocationCache.GetViewLocation) ma non sono riuscito a capire il problema
            viewContext.HttpContext = viewContext.RequestContext.HttpContext;

            LoadParameters(context, viewContext);

            LoadContextVariables(context, viewContext);

            var result = ViewEngines.Engines.FindPartialView(viewContext, GetViewFullName(context));

			if (result.View == null)
				throw new ApplicationException(string.Format("Razor view '{0}' not found.", ViewFile));

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
            Razor.XrcWebViewPageExtension.LoadViewContextVariables(viewContext, context, _moduleCatalog, _moduleFactory);
        }

        /// <summary>
        /// Returns a View name in the razor format, starting from the web site path 
        /// </summary>
        string GetViewFullName(IContext context)
        {
            var viewPath = new Uri(context.GetAbsoluteUrl(ViewFile));
            var appPath = new Uri(context.GetAbsoluteUrl("~"));
            var relative = viewPath.MakeRelativeUriEx(appPath);
            return UriExtensions.Combine(_workingPath.VirtualPath, relative.ToString());
        }
	}
}
