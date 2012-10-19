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
		// Probabilmente si può mettere in cache l'intera IPage con dipendenza a xrcFile.File.FullPath

		public bool ResourceExists(string resourceLocation)
		{
			string file = MapPath(resourceLocation);
			return File.Exists(file);
		}

		public IPage GetPage(Uri url)
		{
			ISiteConfiguration siteConfiguration = _siteConfigurationProvider.GetSiteFromUri(url);
			PageLocatorResult locatorResult = _pageLocator.Locate(siteConfiguration.ToRelativeUrl(url));
			if (locatorResult == null)
				return null;

			PageParserResult parserResult = _pageParser.Parse(locatorResult.Item);

			return new Page(locatorResult.Item, parserResult, locatorResult, siteConfiguration);
		}

		public Stream OpenResource(string resourceLocation)
		{
			string filePath = MapPath(resourceLocation);

			return File.OpenRead(filePath);
		}

		public bool IsDefined(Uri url)
		{
			ISiteConfiguration siteConfiguration = _siteConfigurationProvider.GetSiteFromUri(url);
			PageLocatorResult locatorResult = _pageLocator.Locate(siteConfiguration.ToRelativeUrl(url));

			return locatorResult != null;
		}

		public XDocument ResourceToXml(string resourceLocation)
		{
			using (Stream stream = OpenResource(resourceLocation))
			{
				return XDocument.Load(stream);
			}
		}

		public string ResourceToText(string resourceLocation)
		{
			using (StreamReader stream = new StreamReader(OpenResource(resourceLocation), true))
			{
				string text = stream.ReadToEnd();
				stream.Close();

				return text;
			}
		}

		public byte[] ResourceToBytes(string resourceLocation)
		{
			using (var memStream = new MemoryStream())
			{
				using (var resStream = OpenResource(resourceLocation))
				{
					resStream.CopyTo(memStream);
				}
				return memStream.ToArray();
			}
		}

		public XDocument ResourceToXHtml(string resourceLocation)
		{
			return ResourceToXml(resourceLocation);
		}

		public string ResourceToHtml(string resourceLocation)
		{
			return ResourceToHtml(resourceLocation);
		}

		private string MapPath(string resourceLocation)
		{
			string filePath;
			if (Path.IsPathRooted(resourceLocation))
				filePath = resourceLocation;
			else
				filePath = _workingPath.MapPath(resourceLocation);
			return filePath;
		}
	}
}
