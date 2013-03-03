using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Modules;
using System.Web;

namespace xrc.Pages
{
	public class Page : IPage
	{
		readonly PageActionList _actions;
		readonly PageParameterList _parameters;
		readonly ModuleDefinitionList _modules;
		readonly Configuration.IHostingConfig _hostingConfig;
		readonly XrcUrl _url;
		readonly string _resourceLocation;
		readonly UriSegmentParameterList _urlSegmentsParameters;

        public Page(string resourceLocation, 
                    XrcUrl url,
                    PageActionList actions,
                    PageParameterList parameters,
                    ModuleDefinitionList modules,
                    UriSegmentParameterList urlSegmentsParameters,
					Configuration.IHostingConfig hostingConfig)
		{
			_actions = actions;
			_parameters = parameters;
            _modules = modules;
            _urlSegmentsParameters = urlSegmentsParameters;
            _url = url;
            _resourceLocation = resourceLocation;
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

		public UriSegmentParameterList UrlSegmentsParameters
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
