using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using xrc.Sites;

namespace xrc.Pages.Providers.FileSystem
{
	public class FileSystemPageProviderService : IPageProviderService
	{
		IPageLocatorService _pageLocator;
		IPageParserService _pageParser;
		ISiteConfigurationProviderService _siteConfigurationProvider;

		public FileSystemPageProviderService(IPageLocatorService pageLocator, IPageParserService pageParser, ISiteConfigurationProviderService siteConfigurationProvider)
		{
			_pageLocator = pageLocator;
			_pageParser = pageParser;
			_siteConfigurationProvider = siteConfigurationProvider;
		}

		// TODO Valutare come e se fare cache del risultato di GetPage e IsDefined anche perchè condividono parte del codice.

		public IPage GetPage(Uri url)
		{
			ISiteConfiguration siteConfiguration = _siteConfigurationProvider.GetSiteFromUri(url);
			XrcFile xrcFile = _pageLocator.Locate(siteConfiguration.GetRelativeUrl(url));
			if (xrcFile == null)
				return null;

			PageParserResult parserResult = _pageParser.Parse(xrcFile.FullPath);

			Uri canonicalUrl = siteConfiguration.GetAbsoluteUrl(xrcFile.CanonicalVirtualUrl, url);

			var page = new FileSystemPage(xrcFile, parserResult, siteConfiguration, canonicalUrl);

			return page;
		}

		public Stream GetPageResource(IPage page, string resourceLocation)
		{
			FileSystemPage fsPage = page as FileSystemPage;
			if (fsPage == null)
				throw new XrcException("Invalid page");

			string filePath = GetAbsoluteFile(fsPage, resourceLocation);

			return File.OpenRead(filePath);
		}

		public bool IsDefined(Uri url)
		{
			ISiteConfiguration siteConfiguration = _siteConfigurationProvider.GetSiteFromUri(url);
			XrcFile xrcFile = _pageLocator.Locate(siteConfiguration.GetRelativeUrl(url));

			return xrcFile != null;
		}

		private string GetAbsoluteFile(FileSystemPage page, string resourceLocation)
		{
			if (Path.IsPathRooted(resourceLocation))
				return resourceLocation;
			else
				return Path.Combine(page.File.WorkingPath, resourceLocation);
		}
	}
}
