using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using xrc.Views;
using xrc.Script;
using Moq;
using System.Xml.Linq;
using System.Xml.XPath;
using xrc.Pages.Providers.FileSystem.Parsers;
using xrc.Modules;
using System.IO;

namespace xrc.Pages.Providers.FileSystem
{
	[TestClass]
	public class XHtmlParserService_Test
    {
        [TestInitialize]
        public void Init()
        {
        }
		[TestMethod]
		public void It_Should_be_possible_to_parse_xhtml()
		{
			var file = GetFile(@"sampleWebSite2\conventions_xhtml.xrc.xhtml");
			var viewType = typeof(XHtmlView);

			var schemaParser = new Mock<IXrcSchemaParserService>();
			schemaParser.Setup(p => p.Parse(It.IsAny<string>())).Returns(new PageParserResult());
			var viewCatalog = new Mocks.ViewCatalogServiceMock(new ComponentDefinition(viewType.Name, viewType));

			var target = new XHtmlParserService(schemaParser.Object, viewCatalog);

			PageParserResult page = target.Parse(file);
			var view = page.Actions["GET"].Views.Single();
			Assert.AreEqual(viewType, view.Component.Type);

			var content = (XDocument)view.Properties["Content"].Value.Value;
			Assert.AreEqual("Hello", content.Root.Value);

		}

		[TestMethod]
		public void It_Should_be_possible_to_parse_xhtml_page_with_layout()
		{
			var file = GetFile(@"sampleWebSite1\conventions\page_with_layout.xrc.xhtml");

			var schemaParser = new Mock<IXrcSchemaParserService>();
			schemaParser.Setup(p => p.Parse(It.IsAny<string>())).Returns(new PageParserResult());
			var viewCatalog = new Mocks.ViewCatalogServiceMock(new ComponentDefinition(typeof(XHtmlView).Name, typeof(XHtmlView)));

			var target = new XHtmlParserService(schemaParser.Object, viewCatalog);

			PageParserResult page = target.Parse(file);
			var action = page.Actions["GET"];
			var view = action.Views.Single();
			Assert.AreEqual(typeof(XHtmlView), view.Component.Type);
			Assert.AreEqual("~/shared/_layout", action.Layout);
		}

		[TestMethod]
		public void It_Should_be_possible_to_parse_xhtml_page_without_layout()
		{
			var file = GetFile(@"sampleWebSite1\conventions\_page_without_layout.xrc.xhtml");

			var schemaParser = new Mock<IXrcSchemaParserService>();
			schemaParser.Setup(p => p.Parse(It.IsAny<string>())).Returns(new PageParserResult());
			var viewCatalog = new Mocks.ViewCatalogServiceMock(new ComponentDefinition(typeof(XHtmlView).Name, typeof(XHtmlView)));

			var target = new XHtmlParserService(schemaParser.Object, viewCatalog);

			PageParserResult page = target.Parse(file);
			var action = page.Actions["GET"];
			var view = action.Views.Single();
			Assert.AreEqual(typeof(XHtmlView), view.Component.Type);
			Assert.IsNull(action.Layout);
		}

		private XrcFileResource GetFile(string relativeFilePath)
		{
			string fullPath = TestHelper.GetPath(relativeFilePath);

			var rootConfig = new Mocks.RootPathConfigMock("~/test", Path.GetDirectoryName(fullPath));
			var xrcFolder = new XrcFolder(rootConfig);
			var xrcFile = new XrcFile(xrcFolder, Path.GetFileName(fullPath));
			return new XrcFileResource(xrcFile, "~/test", new Dictionary<string, string>());
		}
    }
}
