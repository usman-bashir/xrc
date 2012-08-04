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
        private IContext _context;
        private System.Web.Mvc.UrlHelper _urlHelper;
        public UrlModule(IContext context)
        {
            _context = context;
            // TODO Valutare se usare UrlHelper o riscirvere i metodi (Action, ...)
            _urlHelper = new System.Web.Mvc.UrlHelper(new XrcRequestContext(context));
        }

        public string Content(string contentPath)
        {
			return _context.GetAbsoluteUrl(contentPath);
        }

        public string Content(string baseUri, string uri)
        {
            return UriExtensions.Combine(Content(baseUri), uri);
        }

        public string Content(string baseUri, string uri1, string uri2)
        {
            return UriExtensions.Combine(UriExtensions.Combine(Content(baseUri), uri1), uri2);
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
