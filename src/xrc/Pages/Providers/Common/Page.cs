using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Modules;
using System.Web;

namespace xrc.Pages.Providers.Common
{
	public class Page : IPage
	{
		readonly PageActionList _actions;
		readonly PageParameterList _parameters;
		readonly ModuleDefinitionList _modules;
		readonly Configuration.IHostingConfig _hostingConfig;
		readonly Sites.ISiteConfiguration _siteConfiguration;
		readonly XrcUrl _url;
		readonly string _resourceLocation;
		readonly Dictionary<string, string> _urlSegmentsParameters;

		public Page(XrcItem item, 
					PageParserResult parserResult,
					PageLocatorResult locatorResult,
					Sites.ISiteConfiguration siteConfiguration,
					Configuration.IHostingConfig hostingConfig)
		{
			_actions = parserResult.Actions;
			_parameters = parserResult.Parameters;
			_modules = parserResult.Modules;
			_siteConfiguration = siteConfiguration;
			_urlSegmentsParameters = locatorResult.UrlSegmentsParameters;
			_url = item.BuildUrl(_urlSegmentsParameters);
			_resourceLocation = item.ResourceLocation;
			_hostingConfig = hostingConfig;
		}

		public PageActionList Actions
		{
			get { return _actions; }
		}

		public PageParameterList PageParameters
		{
			get { return _parameters; }
		}

		public ModuleDefinitionList Modules
		{
			get { return _modules; }
		}

		public Sites.ISiteConfiguration SiteConfiguration
		{
			get { return _siteConfiguration; }
		}

		public Dictionary<string, string> UrlSegmentsParameters
		{
			get { return _urlSegmentsParameters; }
		}

		public XrcUrl PageUrl
		{
			get { return _url; }
		}

		public string ResourceLocation
		{
			get { return _resourceLocation; }
		}

		public string GetResourceLocation(string resourceName)
		{
			return UriExtensions.BuildVirtualPath(ResourceLocation, resourceName);
		}

		public XrcUrl GetPageUrl(string page)
		{
			return new XrcUrl(GetAppRelativeUrl(page));
		}

		public string GetAppRelativeUrl(string resource)
		{
			if (VirtualPathUtility.IsAppRelative(resource))
				return resource;
			else if (VirtualPathUtility.IsAbsolute(resource))
				return _hostingConfig.RelativeUrlToAppRelativeUrl(new Uri(resource, UriKind.RelativeOrAbsolute));
			else
				return UriExtensions.BuildVirtualPath(_url.AppRelaviteUrl, resource);
		}
	}
}
