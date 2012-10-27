using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.IO;

namespace xrc.Modules
{
    public class UrlModule : IUrlModule
    {
        readonly IContext _context;
		readonly System.Web.Mvc.UrlHelper _urlHelper;
		readonly Configuration.IHostingConfig _hostingConfiguration;

		public UrlModule(IContext context, Configuration.IHostingConfig hostingConfiguration)
        {
			_hostingConfiguration = hostingConfiguration;
            _context = context;
            // TODO Valutare se usare UrlHelper o riscrivere i metodi (Action, ...)
            _urlHelper = new System.Web.Mvc.UrlHelper(new XrcRequestContext(context));
        }

		public string Content(string contentPath)
		{
			return _hostingConfiguration.AppRelativeUrlToRelativeUrl(contentPath).ToString();
		}

		public string Content(string contentPathBase, string contentPath)
		{
			return UriExtensions.Combine(Content(contentPathBase), contentPath);
		}

		public string Current()
		{
			return _context.Request.Url.ToString();
		}

		public string Initiator()
		{
			return _context.GetInitiatorContext().Request.Url.ToString();
		}

		public bool IsBaseOf(string baseUrl, string url)
		{
			Uri baseUrlUri = new Uri(baseUrl);
			return baseUrlUri.IsBaseOfWithPath(new Uri(url));
		}

        public string MvcAction(string actionName, string controllerName)
        {
            return _urlHelper.Action(actionName, controllerName);
        }

        public string MvcAction(string actionName, string controllerName, object routeValues)
        {
            return _urlHelper.Action(actionName, controllerName, routeValues);
        }

        public string MvcAction(string actionName, string controllerName, System.Web.Routing.RouteValueDictionary routeValues)
        {
            return _urlHelper.Action(actionName, controllerName, routeValues);
        }

        public string MvcAction(string actionName, string controllerName, object routeValues, string protocol)
        {
            return _urlHelper.Action(actionName, controllerName, routeValues, protocol);
        }

        public string MvcAction(string actionName, string controllerName, System.Web.Routing.RouteValueDictionary routeValues, string protocol, string hostName)
        {
            return _urlHelper.Action(actionName, controllerName, routeValues, protocol, hostName);
        }
	}
}
