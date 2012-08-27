using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using xrc.Sites;

namespace xrc.Pages.Providers.FileSystem
{
	[TestClass]
	public class FileSystemPageProviderService_Test
    {
        [TestInitialize]
        public void Init()
        {
        }

        [TestMethod]
        public void It_Should_be_possible_get_a_valid_page()
        {
			var parserResult = new PageParserResult();
			var pageParser = new Mock<IPageParserService>();
			pageParser.Setup(p => p.Parse(It.IsAny<XrcFile>())).Returns(parserResult);

			var xrcFolder = new XrcFolder(@"c:\temp", null);
			var xrcFile = new XrcFile(@"c:\temp\test.xrc", xrcFolder, "~/test", new Dictionary<string, string>());
			var pageLocator = new Mock<IPageLocatorService>();
			pageLocator.Setup(p => p.Locate(It.IsAny<Uri>())).Returns(xrcFile);
	
			SiteConfiguration siteConfiguration = new SiteConfiguration("test", new Uri("http://test.com"));
			var siteConfigurationProvider = new Mock<ISiteConfigurationProviderService>();
			siteConfigurationProvider.Setup(p => p.GetSiteFromUri(It.IsAny<Uri>())).Returns(siteConfiguration);

			FileSystemPageProviderService target = new FileSystemPageProviderService(pageLocator.Object, pageParser.Object, siteConfigurationProvider.Object);

			var url = new Uri("http://test.com/test");

			Assert.AreEqual(true, target.IsDefined(url));

			var page = target.GetPage(url);

			Assert.IsNotNull(page);
			Assert.AreEqual(parserResult.Modules, page.Modules);
			Assert.AreEqual(parserResult.Actions, page.Actions);
			Assert.AreEqual(parserResult.Parameters, page.PageParameters);
			Assert.AreEqual(siteConfiguration, page.SiteConfiguration);

			pageLocator.Verify(p => p.Locate(new Uri("test", UriKind.Relative)));
			pageParser.Verify(p => p.Parse(xrcFile));
			siteConfigurationProvider.Verify(p => p.GetSiteFromUri(url));
        }

		[TestMethod]
		public void It_Should_be_possible_get_a_not_found_page()
		{
			var pageParser = new Mock<IPageParserService>();
			var pageLocator = new Mock<IPageLocatorService>();
			var siteConfigurationProvider = new Mock<ISiteConfigurationProviderService>();
			pageLocator.Setup(p => p.Locate(It.IsAny<Uri>())).Returns((XrcFile)null);
			SiteConfiguration siteConfiguration = new SiteConfiguration("test", new Uri("http://test.com"));
			siteConfigurationProvider.Setup(p => p.GetSiteFromUri(It.IsAny<Uri>())).Returns(siteConfiguration);

			FileSystemPageProviderService target = new FileSystemPageProviderService(pageLocator.Object, pageParser.Object, siteConfigurationProvider.Object);

			var url = new Uri("http://test.com/test");

			Assert.AreEqual(false, target.IsDefined(url));

			var page = target.GetPage(url);

			Assert.IsNull(page);

			pageLocator.Verify(p => p.Locate(It.IsAny<Uri>()));
			siteConfigurationProvider.Verify(p => p.GetSiteFromUri(It.IsAny<Uri>()));
			pageParser.Verify(p => p.Parse(It.IsAny<XrcFile>()), Times.Never());
		}
	}
}
