using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using xrc.Sites;
using System.Web;

namespace xrc.Pages.Providers.FileSystem
{
	public class FileSystemPageProviderService : IPageProviderService
	{
		readonly IPageLocatorService _pageLocator;
		readonly IPageParserService _pageParser;
		readonly ISiteConfigurationProviderService _siteConfigurationProvider;
		readonly Configuration.IRootPathConfig _workingPath;

		public FileSystemPageProviderService(Configuration.IRootPathConfig workingPath, IPageLocatorService pageLocator, IPageParserService pageParser, ISiteConfigurationProviderService siteConfigurationProvider)
		{
			_workingPath = workingPath;
			_pageLocator = pageLocator;
			_pageParser = pageParser;
			_siteConfigurationProvider = siteConfigurationProvider;
		}

		// TODO Valutare come e se fare cache del risultato di GetPage e IsDefined anche perchè condividono parte del codice.

		public IPage GetPage(Uri url)
		{
			ISiteConfiguration siteConfiguration = _siteConfigurationProvider.GetSiteFromUri(url);
			XrcFileResource xrcFile = _pageLocator.Locate(siteConfiguration.ToRelativeUrl(url));
			if (xrcFile == null)
				return null;

			PageParserResult parserResult = _pageParser.Parse(xrcFile);

			Uri canonicalUrl = siteConfiguration.ToAbsoluteUrl(xrcFile.CanonicalVirtualUrl, url);

			return new FileSystemPage(xrcFile, parserResult, siteConfiguration, canonicalUrl);
		}

		public Stream OpenPageResource(IPage page, string resourceLocation)
		{
			FileSystemPage fsPage = page as FileSystemPage;
			if (fsPage == null)
				throw new XrcException("Invalid page");

			string filePath = GetAbsoluteFile(fsPage, resourceLocation);

			return File.OpenRead(filePath);
		}

		public string GetPageVirtualPath(IPage page, string url)
		{
			FileSystemPage fsPage = page as FileSystemPage;
			if (fsPage == null)
				throw new XrcException("Invalid page");

			if (VirtualPathUtility.IsAbsolute(url))
				return url;

			return VirtualPathUtility.Combine(fsPage.FileResource.VirtualPath, url);

			//var viewPath = page.ToAbsoluteUrl(url);
			//var appPath = page.ToAbsoluteUrl("~");
			//var relative = viewPath.MakeRelativeUriEx(appPath);
			//return UriExtensions.Combine(_workingPath.VirtualPath, relative.ToString());
		}

		public bool IsDefined(Uri url)
		{
			ISiteConfiguration siteConfiguration = _siteConfigurationProvider.GetSiteFromUri(url);
			XrcFileResource xrcFile = _pageLocator.Locate(siteConfiguration.ToRelativeUrl(url));

			return xrcFile != null;
		}

		private string GetAbsoluteFile(FileSystemPage page, string resourceLocation)
		{
			if (Path.IsPathRooted(resourceLocation))
				return resourceLocation;
			else
				return Path.Combine(page.FileResource.WorkingPath, resourceLocation);
		}
	}
}
