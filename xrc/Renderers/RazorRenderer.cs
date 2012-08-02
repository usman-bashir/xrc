using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.IO;
using System.Web;

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
		public RazorRenderer(IKernel kernel, Configuration.WorkingPath workingPath)
		{
			_kernel = kernel;
            _workingPath = workingPath;
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
            viewContext.RequestContext = new System.Web.Routing.RequestContext();
            viewContext.HttpContext = new RazorHttpContext(context);

            // Load parameters
            foreach (var param in context.Parameters)
                viewContext.ViewData.Add(param.Name, param.Value);

			// TODO bisogna dalla pagina Razor poter accedere ai moduli (forse utilizzando le @functions razor e caricando i moduli al volo?)

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

        /// <summary>
        /// Returns a View name in the razor format, starting from the web site path 
        /// </summary>
        /// <returns></returns>
        string GetViewFullName(IContext context)
        {
            var viewPath = new Uri(context.GetAbsoluteUrl(View));
            var appPath = new Uri(context.GetAbsoluteUrl("~"));
            var relative = viewPath.MakeRelativeUriEx(appPath);
            return UriExtensions.Combine(_workingPath.VirtualPath, relative.ToString());
        }

		class RazorHttpContext : HttpContextBase
		{
			private IContext _context;
			private Dictionary<object, object> _items = new Dictionary<object, object>();

			public RazorHttpContext(IContext context)
			{
				_context = context;
			}

			public override HttpRequestBase Request
			{
				get
				{
					return _context.Request;
				}
			}

			public override System.Collections.IDictionary Items
			{
				get
				{
					return _items;
				}
			}
		}
	}
}
