using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using xrc.Sites;
using System.Web;
using xrc.Pages.Providers.Common;
using System.Xml.Linq;

namespace xrc.Pages.Providers.FileSystem
{
	public class FileSystemPageProviderService : IPageProviderService
	{
		readonly IPageLocatorService _pageLocator;
		readonly IPageParserService _pageParser;

		public FileSystemPageProviderService(IPageLocatorService pageLocator, IPageParserService pageParser)
		{
			_pageLocator = pageLocator;
			_pageParser = pageParser;
		}

		// TODO Valutare come e se fare cache del risultato di GetPage e IsDefined anche perchè condividono parte del codice.
		// Probabilmente si può mettere in cache l'intera IPage con dipendenza a xrcFile.File.FullPath

		public bool PageExists(XrcUrl url)
		{
			PageLocatorResult locatorResult = _pageLocator.Locate(url);

			return locatorResult != null;
		}

		public IPage GetPage(XrcUrl url, Sites.ISiteConfiguration siteConfiguration)
		{
			PageLocatorResult locatorResult = _pageLocator.Locate(url);
			if (locatorResult == null)
				return null;

			PageParserResult parserResult = _pageParser.Parse(locatorResult.Item);

			return new Page(locatorResult.Item, parserResult, locatorResult, siteConfiguration);
		}
	}
}
