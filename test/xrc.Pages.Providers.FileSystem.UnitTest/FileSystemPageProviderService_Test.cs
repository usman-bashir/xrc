using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using xrc.Pages.Parsers;
using xrc.Configuration;
using xrc.Pages.TreeStructure;

namespace xrc.Pages.Providers.FileSystem
{
    [TestClass]
    public class FileSystemPageProviderService_Test
    {
        Mock<IPageParserService> _pageParser;
        Mock<IPageLocatorService> _pageLocator;

        [TestInitialize]
        public void Init()
        {
        }

        FileSystemPageProviderService CreateTarget()
        {
            _pageParser = new Mock<IPageParserService>();
            _pageLocator = new Mock<IPageLocatorService>();

            var hostingConfig = new Mock<IHostingConfig>();

            return new FileSystemPageProviderService(_pageLocator.Object, _pageParser.Object, hostingConfig.Object);
        }

        private void SetupUrl(XrcUrl url, PageLocatorResult locatorResult, PageDefinition parserResult)
        {
            _pageLocator.Setup(p => p.Locate(url)).Returns(locatorResult);

            if (locatorResult != null)
                _pageParser.Setup(p => p.Parse(locatorResult.Page.ResourceLocation)).Returns(parserResult);
        }

        [TestMethod]
        public void It_Should_be_possible_get_a_not_found_page()
        {
            var url = new XrcUrl("~/test");
            var target = CreateTarget();

            SetupUrl(url, null, null);

            Assert.AreEqual(false, target.PageExists(url));

            var page = target.GetPage(url);

            Assert.IsNull(page);

            _pageLocator.Verify(p => p.Locate(url));
        }

        [TestMethod]
        public void It_Should_be_possible_to_parse_folder_parameters()
        {
            var root = new TestPageStructure().GetRoot();
            var config = root.ConfigFile;
            var file = root.Files["about.xrc"];

            var configParserResult = new PageDefinition();
            configParserResult.Parameters.Add(new PageParameter("folderParameter1", new XValue("folder")));
            configParserResult.Parameters.Add(new PageParameter("folderParameter2", new XValue("folder")));

            var pageParserResult = new PageDefinition();
            pageParserResult.Parameters.Add(new PageParameter("folderParameter2", new XValue("page")));

            var target = CreateTarget();
            SetupUrl(file.BuildUrl(), new PageLocatorResult(file, new UriSegmentParameterList()), pageParserResult);
            _pageParser.Setup(p => p.Parse(config.ResourceLocation)).Returns(configParserResult);

            var page = target.GetPage(file.BuildUrl());
            Assert.AreEqual("folder", page.PageParameters["folderParameter1"].Value.ToString());
            Assert.AreEqual("page", page.PageParameters["folderParameter2"].Value.ToString());
        }

        [TestMethod]
        public void It_Should_be_possible_to_parse_page_and_get_default_layout()
        {
            var root = new TestPageStructure().GetRoot();
            var layout = root.DefaultLayoutFile;
            var config = root.ConfigFile; 
            var file = root.Files["about.xrc"];

            var pageParserResult = new PageDefinition();
            pageParserResult.Actions.Add(new PageAction("GET"));
            pageParserResult.Actions.Add(new PageAction("POST"));

            var target = CreateTarget();
            SetupUrl(file.BuildUrl(), new PageLocatorResult(file, new UriSegmentParameterList()), pageParserResult);
            _pageParser.Setup(p => p.Parse(config.ResourceLocation)).Returns(new PageDefinition());

            var page = target.GetPage(file.BuildUrl());
            var actionGet = page.Actions["get"];
            Assert.AreEqual("~/_layout", actionGet.Layout);

            var actionPost = page.Actions["post"];
            Assert.IsNull(actionPost.Layout);
        }

        // TODO Add more test for nested directory (layout and config)
    }
}
