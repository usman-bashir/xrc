using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xrc.Modules;
using System.Web;

namespace xrc.Pages.Providers.FileSystem
{
	public class FileSystemPage : IPage
	{
		readonly PageActionList _actions;
		readonly PageParameterList _parameters;
		readonly ModuleDefinitionList _modules;
		readonly XrcFileResource _fileResource;
		readonly Sites.ISiteConfiguration _siteConfiguration;
		readonly Uri _canonicalUrl;

		public FileSystemPage(XrcFileResource fileResource, 
							PageParserResult parserResult,
							Sites.ISiteConfiguration siteConfiguration,
							Uri canonicalUrl)
		{
			_actions = parserResult.Actions;
			_parameters = parserResult.Parameters;
			_modules = parserResult.Modules;
			_fileResource = fileResource;
			_siteConfiguration = siteConfiguration;
			_canonicalUrl = canonicalUrl;
		}

		public XrcFile File
		{
			get { return FileResource.File; }
		}

		public XrcFileResource FileResource
		{
			get { return _fileResource; }
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
			get { return _fileResource.UrlSegmentsParameters; }
		}

		public Uri ToAbsoluteUrl(string relativeUrl)
		{
			return SiteConfiguration.ToAbsoluteUrl(relativeUrl, _canonicalUrl);
		}

		public bool IsCanonicalUrl(Uri url)
		{
			// UrlDecode to support { and } characters that in some cases are encoded by web servers
			string currentUrl = HttpUtility.UrlDecode(url.GetLeftPart(UriPartial.Path));

			return string.Equals(_canonicalUrl.ToString(), currentUrl, StringComparison.Ordinal);
		}
	}
}
