using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Modules;

namespace xrc.Pages.Providers.FileSystem
{
	public class FileSystemPage : IPage
	{
		readonly PageActionList _actions;
		readonly PageParameterList _parameters;
		readonly ModuleDefinitionList _modules;
		readonly XrcFile _file;
		readonly Sites.ISiteConfiguration _siteConfiguration;
		readonly Uri _canonicalUrl;

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

		public Uri ToAbsoluteUrl(string relativeUrl)
		{
			return SiteConfiguration.ToAbsoluteUrl(relativeUrl, _canonicalUrl);
		}
	}
}
