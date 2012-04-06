using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.IO;
using System.Web;

namespace xrc.Renderers
{
    public class RazorRenderer : IRenderer
    {
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

			// TODO Rivedere questo codice di chiamata a razor. 
			// Alternative:
			// http://razorengine.codeplex.com/
			// http://vibrantcode.com/blog/2010/11/16/hosting-razor-outside-of-aspnet-revised-for-mvc3-rc.html
			ControllerContext controllerContext = new ControllerContext();
			controllerContext.RouteData.Values.Add("controller", "RazorRenderer");
			controllerContext.RequestContext = new System.Web.Routing.RequestContext();
			controllerContext.HttpContext = new RazorHttpContext(context);

			// TODO Fare in modo di accettare anche path relativi
			//string fullViewName = Path.Combine(context.WorkingPath, View);
			var result = ViewEngines.Engines.FindPartialView(controllerContext, View);

			if (result.View == null)
				throw new ApplicationException(string.Format("Razor view '{0}' not found.", View));

			ViewContext viewContext = new ViewContext();
			viewContext.ViewData = new ViewDataDictionary(Model);
			viewContext.HttpContext = controllerContext.HttpContext;
			result.View.Render(viewContext, context.Response.Output);
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
