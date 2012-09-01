using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Modules;

namespace xrc.Pages.Providers.FileSystem
{
	public class FileSystemPage : IPage
	{
		PageActionList _actions;
		PageParameterList _parameters;
		ModuleDefinitionList _modules;
		XrcFile _file;
		Sites.ISiteConfiguration _siteConfiguration;
		Uri _canonicalUrl;

		public FileSystemPage(XrcFile file, 
							PageParserResult parserResult,
							Sites.ISiteConfiguration siteConfiguration, 
							Uri canonicalUrl)
		{
			_actions = parserResult.Actions;
			_parameters = parserResult.Parameters;
			_modules = parserResult.Modules;
			_file = file;
			_siteConfiguration = siteConfiguration;
			_canonicalUrl = canonicalUrl;
		}

		public XrcFile File
		{
			get { return _file; }
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

		public Uri CanonicalUrl
		{
			get { return _canonicalUrl; }
		}

		public Dictionary<string, string> UrlSegmentsParameters
		{
			get { return _file.UrlSegmentsParameters; }
		}

		public Uri GetContentAbsoluteUrl(string contentUrlPageRelative)
		{
			return SiteConfiguration.GetAbsoluteUrl(contentUrlPageRelative, _canonicalUrl);
		}
	}
}
