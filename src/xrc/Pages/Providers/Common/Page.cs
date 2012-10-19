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
		readonly string _Id;
		readonly PageActionList _actions;
		readonly PageParameterList _parameters;
		readonly ModuleDefinitionList _modules;
		readonly Sites.ISiteConfiguration _siteConfiguration;
		readonly string _virtualPath;
		readonly string _physicalVirtualPath;
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
			_physicalVirtualPath = item.VirtualPath;
			_Id = item.Id;
		}

		public string Id
		{
			get { return _Id; }
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

		/// <summary>
		/// This is a virtual path with the parameters resolved. Example: ~/athletes/phelps/index
		/// </summary>
		public string LogicalVirtualPath
		{
			get { return _virtualPath; }
		}

		/// <summary>
		/// This is a virtual path with the parameters not resolved. Example: ~/athletes/{name}/index
		/// </summary>
		public string PhysicalVirtualPath
		{
			get { return _physicalVirtualPath; }
		}

		public Dictionary<string, string> UrlSegmentsParameters
		{
			get { return _urlSegmentsParameters; }
		}

		public string ToVirtualUrl(string relativeUrl, ContentUrlMode mode)
		{
			switch (mode)
			{
				case ContentUrlMode.Logical:
					return UriExtensions.BuildVirtualPath(LogicalVirtualPath, relativeUrl);
				case ContentUrlMode.Physical:
					return UriExtensions.BuildVirtualPath(PhysicalVirtualPath, relativeUrl);
				default:
					throw new XrcException(string.Format("Mode '{0}' not valid.", mode));
			}
		}
		public Uri ToAbsoluteUrl(string relativeUrl, ContentUrlMode mode)
		{
			string virtualUrl = ToVirtualUrl(relativeUrl, mode);
			return SiteConfiguration.VirtualUrlToAbsolute(virtualUrl);
		}

		public bool IsCanonicalUrl(Uri url)
		{
			Uri expectedUrl = _siteConfiguration.VirtualUrlToRelative(LogicalVirtualPath);
			Uri actualUrl = _siteConfiguration.AbsoluteUrlToRelative(url);

			return string.Equals(expectedUrl.ToString(), actualUrl.ToString(), StringComparison.Ordinal);
		}
	}
}
