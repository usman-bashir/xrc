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
		readonly Sites.ISiteConfiguration _siteConfiguration;
		readonly string _virtualPath;
		readonly string _resourceLocation;
		readonly Dictionary<string, string> _urlSegmentsParameters;

		public Page(XrcItem item, 
					PageParserResult parserResult,
					PageLocatorResult locatorResult,
					Sites.ISiteConfiguration siteConfiguration)
		{
			_actions = parserResult.Actions;
			_parameters = parserResult.Parameters;
			_modules = parserResult.Modules;
			_siteConfiguration = siteConfiguration;
			_urlSegmentsParameters = locatorResult.UrlSegmentsParameters;
			_virtualPath = locatorResult.VirtualPath;
			_resourceLocation = item.ResourceLocation;
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

		public string VirtualPath
		{
			get { return _virtualPath; }
		}

		public string ResourceLocation
		{
			get { return _resourceLocation; }
		}

		public string GetResourceLocation(string resourceName)
		{
			return UriExtensions.BuildVirtualPath(ResourceLocation, resourceName);
		}

		public string GetContentVirtualUrl(string page)
		{
			return UriExtensions.BuildVirtualPath(VirtualPath, page);
		}
	}
}
