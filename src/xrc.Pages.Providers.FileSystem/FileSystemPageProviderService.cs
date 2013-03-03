using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Xml.Linq;
using xrc.Configuration;
using xrc.Pages.TreeStructure;
using xrc.Pages.Parsers;

namespace xrc.Pages.Providers.FileSystem
{
	public class FileSystemPageProviderService : IPageProviderService
	{
		readonly IPageLocatorService _pageLocator;
		readonly IPageParserService _pageParser;
		readonly IHostingConfig _hostingConfig;

		public FileSystemPageProviderService(IPageLocatorService pageLocator, IPageParserService pageParser, IHostingConfig hostingConfig)
		{
			_pageLocator = pageLocator;
			_pageParser = pageParser;
			_hostingConfig = hostingConfig;
		}

		// TODO Valutare come e se fare cache del risultato di GetPage e IsDefined anche perchè condividono parte del codice.
		// Probabilmente si può mettere in cache l'intera IPage con dipendenza al file

		public bool PageExists(XrcUrl url)
		{
			PageLocatorResult locatorResult = _pageLocator.Locate(url);

			return locatorResult != null;
		}

		public IPage GetPage(XrcUrl url)
		{
			PageLocatorResult locatorResult = _pageLocator.Locate(url);
			if (locatorResult == null)
				return null;

            PageFile page = locatorResult.Page;
            PageDefinition pageDefinition = GetPageDefinition(page);

            SetLayout(pageDefinition, page, locatorResult.UrlSegmentsParameters);

            var pageUrl = page.BuildUrl(locatorResult.UrlSegmentsParameters);

			return new Page(page.ResourceLocation,
                            pageUrl,
                            pageDefinition.Actions,
                            pageDefinition.Parameters,
                            pageDefinition.Modules,
                            locatorResult.UrlSegmentsParameters,
                            _hostingConfig);
		}

        void SetLayout(PageDefinition pageDefinition, PageFile pageFile, UriSegmentParameterList uriParameters)
        {
            if (!pageDefinition.Actions.Contains("GET"))
                return;

            var action = pageDefinition.Actions["GET"];
            if (action != null)
                action.Layout = pageFile.DefaultLayoutFile.BuildUrl(uriParameters).ToString();
        }

        PageDefinition GetPageDefinition(PageFile page)
        {
            var pageDefinition = _pageParser.Parse(page.ResourceLocation);

            var inheritedDefinitions = GetInheritedDefinitions(page.Parent);

            return inheritedDefinitions.Combine(pageDefinition);
        }

        PageDefinition GetInheritedDefinitions(PageDirectory directory)
        {
            PageDefinition directoryDefinition;
            if (directory.ConfigFile != null)
                directoryDefinition = _pageParser.Parse(directory.ConfigFile.ResourceLocation);
            else
                directoryDefinition = new PageDefinition();

            if (directory.Parent != null)
                return GetInheritedDefinitions(directory.Parent).Combine(directoryDefinition);

            return directoryDefinition;
        }
	}
}
