using System;
using System.Web.Routing;
namespace xrc.Modules
{
    public interface IUrlModule : IModule
    {
        /// <summary>
        /// Converts a virtual (relative) path to an application absolute path.
        /// </summary>
        /// <param name="contentPath">The virtual path of the content.</param>
        /// <returns>The application absolute path.</returns>
        string Content(string baseUri, string uri);
        string Content(string baseUri, string uri1, string uri2);
        string Content(string contentPath);

        /// <summary>
        /// Generates a fully qualified URL to an action method by using the specified action name and controller name.
        /// </summary>
        /// <param name="actionName">The name of the action method.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <returns>The fully qualified URL to an action method.</returns>
        string MvcAction(string actionName, string controllerName);

        /// <summary>
        /// Generates a fully qualified URL to an action method by using the specified action name, controller name, and route values.
        /// </summary>
        /// <param name="actionName">The name of the action method.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">An object that contains the parameters for a route. The parameters are retrieved through reflection by examining the properties of the object. The object is typically created by using object initializer syntax.</param>
        /// <returns>The fully qualified URL to an action method.</returns>
        string MvcAction(string actionName, string controllerName, object routeValues);

        /// <summary>
        /// Generates a fully qualified URL to an action method by using the specified action name, controller name, and route values.
        /// </summary>
        /// <param name="actionName">The name of the action method.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">An object that contains the parameters for a route.</param>
        /// <returns>The fully qualified URL to an action method.</returns>
        string MvcAction(string actionName, string controllerName, RouteValueDictionary routeValues);

        /// <summary>
        /// Generates a fully qualified URL for an action method by using the specified action name, controller name, route values, and protocol to use.
        /// </summary>
        /// <param name="actionName">The name of the action method.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">An object that contains the parameters for a route. The parameters are retrieved through reflection by examining the properties of the object. The object is typically created by using object initializer syntax.</param>
        /// <param name="protocol">The protocol for the URL, such as "http" or "https".</param>
        /// <returns>The fully qualified URL to an action method.</returns>
        string MvcAction(string actionName, string controllerName, object routeValues, string protocol);

        /// <summary>
        /// Generates a fully qualified URL for an action method by using the specified action name, controller name, route values, protocol to use, and host name.
        /// </summary>
        /// <param name="actionName">The name of the action method.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">An object that contains the parameters for a route.</param>
        /// <param name="protocol">The protocol for the URL, such as "http" or "https".</param>
        /// <param name="hostName">The host name for the URL.</param>
        /// <returns>The fully qualified URL to an action method.</returns>
        string MvcAction(string actionName, string controllerName, RouteValueDictionary routeValues, string protocol, string hostName);
    }
}
