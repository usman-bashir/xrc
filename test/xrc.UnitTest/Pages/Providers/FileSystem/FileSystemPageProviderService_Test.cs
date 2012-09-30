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
        public void It_Should_be_possible_get_a_valid_page_on_the_root()
        {
			var parserResult = new PageParserResult();
			var pageParser = new Mock<IPageParserService>();
			pageParser.Setup(p => p.Parse(It.IsAny<XrcFileResource>())).Returns(parserResult);

			var workingPath = new Mocks.RootPathConfigMock("~/sampleWebSite1", TestHelper.GetPath("sampleWebSite1"));
			var xrcFolder = new XrcFolder(workingPath);
			var xrcFile = new XrcFile(xrcFolder, "about.xrc");
			var xrcFileResource = new XrcFileResource(xrcFile, "~/about", new Dictionary<string, string>());
			var pageLocator = new Mock<IPageLocatorService>();
			pageLocator.Setup(p => p.Locate(It.IsAny<Uri>())).Returns(xrcFileResource);
	
			SiteConfiguration siteConfiguration = new SiteConfiguration("test", new Uri("http://test.com"));
			var siteConfigurationProvider = new Mock<ISiteConfigurationProviderService>();
			siteConfigurationProvider.Setup(p => p.GetSiteFromUri(It.IsAny<Uri>())).Returns(siteConfiguration);

			FileSystemPageProviderService target = new FileSystemPageProviderService(workingPath, pageLocator.Object, pageParser.Object, siteConfigurationProvider.Object);

			var url = new Uri("http://test.com/about");

			Assert.AreEqual(true, target.IsDefined(url));

			var page = target.GetPage(url);

			Assert.IsNotNull(page);
			Assert.AreEqual(parserResult.Modules, page.Modules);
			Assert.AreEqual(parserResult.Actions, page.Actions);
			Assert.AreEqual(parserResult.Parameters, page.PageParameters);
			Assert.AreEqual(siteConfiguration, page.SiteConfiguration);

			pageLocator.Verify(p => p.Locate(new Uri("about", UriKind.Relative)));
			pageParser.Verify(p => p.Parse(xrcFileResource));
			siteConfigurationProvider.Verify(p => p.GetSiteFromUri(url));

			Assert.AreEqual("~/samplewebsite1/file.cshtml", target.GetPageVirtualPath(page, "file.cshtml"));
		}

		[TestMethod]
		public void It_Should_be_possible_get_a_valid_page_on_a_folder()
		{
			var parserResult = new PageParserResult();
			var pageParser = new Mock<IPageParserService>();
			pageParser.Setup(p => p.Parse(It.IsAny<XrcFileResource>())).Returns(parserResult);

			var workingPath = new Mocks.RootPathConfigMock("~/sampleWebSite1", TestHelper.GetPath("sampleWebSite1"));
			var xrcFolder = new XrcFolder(workingPath).GetFolder("teams");
			var xrcFile = new XrcFile(xrcFolder, "index.xrc");
			var xrcFileResource = new XrcFileResource(xrcFile, "~/teams/", new Dictionary<string, string>());
			var pageLocator = new Mock<IPageLocatorService>();
			pageLocator.Setup(p => p.Locate(It.IsAny<Uri>())).Returns(xrcFileResource);

			SiteConfiguration siteConfiguration = new SiteConfiguration("test", new Uri("http://test.com"));
			var siteConfigurationProvider = new Mock<ISiteConfigurationProviderService>();
			siteConfigurationProvider.Setup(p => p.GetSiteFromUri(It.IsAny<Uri>())).Returns(siteConfiguration);

			FileSystemPageProviderService target = new FileSystemPageProviderService(workingPath, pageLocator.Object, pageParser.Object, siteConfigurationProvider.Object);

			var url = new Uri("http://test.com/teams/");

			Assert.AreEqual(true, target.IsDefined(url));

			var page = target.GetPage(url);

			Assert.IsNotNull(page);
			Assert.AreEqual(parserResult.Modules, page.Modules);
			Assert.AreEqual(parserResult.Actions, page.Actions);
			Assert.AreEqual(parserResult.Parameters, page.PageParameters);
			Assert.AreEqual(siteConfiguration, page.SiteConfiguration);

			pageLocator.Verify(p => p.Locate(new Uri("teams/", UriKind.Relative)));
			pageParser.Verify(p => p.Parse(xrcFileResource));
			siteConfigurationProvider.Verify(p => p.GetSiteFromUri(url));

			Assert.AreEqual("~/samplewebsite1/teams/file.cshtml", target.GetPageVirtualPath(page, "file.cshtml"));
		}

		[TestMethod]
		public void It_Should_be_possible_get_a_valid_page_on_a_parametrized_folder()
		{
			var parserResult = new PageParserResult();
			var pageParser = new Mock<IPageParserService>();
			pageParser.Setup(p => p.Parse(It.IsAny<XrcFileResource>())).Returns(parserResult);

			var workingPath = new Mocks.RootPathConfigMock("~/sampleWebSite1", TestHelper.GetPath("sampleWebSite1"));
			var xrcFolder = new XrcFolder(workingPath).GetFolder("teams").GetFolder("{teamid}");
			var xrcFile = new XrcFile(xrcFolder, "matches.xrc");
			var xrcFileResource = new XrcFileResource(xrcFile, "~/teams/torino/matches", new Dictionary<string, string>());
			var pageLocator = new Mock<IPageLocatorService>();
			pageLocator.Setup(p => p.Locate(It.IsAny<Uri>())).Returns(xrcFileResource);

			SiteConfiguration siteConfiguration = new SiteConfiguration("test", new Uri("http://test.com"));
			var siteConfigurationProvider = new Mock<ISiteConfigurationProviderService>();
			siteConfigurationProvider.Setup(p => p.GetSiteFromUri(It.IsAny<Uri>())).Returns(siteConfiguration);

			FileSystemPageProviderService target = new FileSystemPageProviderService(workingPath, pageLocator.Object, pageParser.Object, siteConfigurationProvider.Object);

			var url = new Uri("http://test.com/teams/TORINO/matches");

			Assert.AreEqual(true, target.IsDefined(url));

			var page = target.GetPage(url);

			Assert.IsNotNull(page);
			Assert.AreEqual(parserResult.Modules, page.Modules);
			Assert.AreEqual(parserResult.Actions, page.Actions);
			Assert.AreEqual(parserResult.Parameters, page.PageParameters);
			Assert.AreEqual(siteConfiguration, page.SiteConfiguration);

			pageLocator.Verify(p => p.Locate(new Uri("teams/torino/matches", UriKind.Relative)));
			pageParser.Verify(p => p.Parse(xrcFileResource));
			siteConfigurationProvider.Verify(p => p.GetSiteFromUri(url));

			Assert.AreEqual("~/samplewebsite1/teams/{teamid}/file.cshtml", target.GetPageVirtualPath(page, "file.cshtml"));
		}

		[TestMethod]
		public void It_Should_be_possible_get_a_not_found_page()
		{
			var pageParser = new Mock<IPageParserService>();
			var pageLocator = new Mock<IPageLocatorService>();
			var siteConfigurationProvider = new Mock<ISiteConfigurationProviderService>();
			pageLocator.Setup(p => p.Locate(It.IsAny<Uri>())).Returns((XrcFileResource)null);
			SiteConfiguration siteConfiguration = new SiteConfiguration("test", new Uri("http://test.com"));
			siteConfigurationProvider.Setup(p => p.GetSiteFromUri(It.IsAny<Uri>())).Returns(siteConfiguration);

			var rootPathConfig = new Mocks.RootPathConfigMock("~/", "c:\temp");

			FileSystemPageProviderService target = new FileSystemPageProviderService(rootPathConfig, pageLocator.Object, pageParser.Object, siteConfigurationProvider.Object);

			var url = new Uri("http://test.com/test");

			Assert.AreEqual(false, target.IsDefined(url));

			var page = target.GetPage(url);

			Assert.IsNull(page);

			pageLocator.Verify(p => p.Locate(It.IsAny<Uri>()));
			siteConfigurationProvider.Verify(p => p.GetSiteFromUri(It.IsAny<Uri>()));
			pageParser.Verify(p => p.Parse(It.IsAny<XrcFileResource>()), Times.Never());
		}
	}
}
