using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace xrc.Modules
{
    public class UrlMVCModule : IUrlMVCModule
    {
		readonly UrlHelper _urlHelper;

		public UrlMVCModule(IContext context)
        {
            _urlHelper = new UrlHelper(new XrcRequestContext(context));
        }

        public string Action(string actionName, string controllerName)
        {
            return _urlHelper.Action(actionName, controllerName);
        }

        public string Action(string actionName, string controllerName, object routeValues)
        {
            return _urlHelper.Action(actionName, controllerName, routeValues);
        }

        public string Action(string actionName, string controllerName, System.Web.Routing.RouteValueDictionary routeValues)
        {
            return _urlHelper.Action(actionName, controllerName, routeValues);
        }

        public string Action(string actionName, string controllerName, object routeValues, string protocol)
        {
            return _urlHelper.Action(actionName, controllerName, routeValues, protocol);
        }

        public string Action(string actionName, string controllerName, System.Web.Routing.RouteValueDictionary routeValues, string protocol, string hostName)
        {
            return _urlHelper.Action(actionName, controllerName, routeValues, protocol, hostName);
        }
	}
}
