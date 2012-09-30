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
	public class MarkdownParserService_Test
    {
        [TestInitialize]
        public void Init()
        {
        }
		[TestMethod]
		public void It_Should_be_possible_to_parse_html()
		{
			var file = GetFile(@"sampleWebSite2\conventions_markdown.xrc.md");
			var viewType = typeof(HtmlView);

			var schemaParser = new Mock<IXrcSchemaParserService>();
			schemaParser.Setup(p => p.Parse(It.IsAny<string>())).Returns(new PageParserResult());
			var viewCatalog = new Mocks.ViewCatalogServiceMock(new ComponentDefinition(viewType.Name, viewType));

			var target = new MarkdownParserService(schemaParser.Object, viewCatalog);

			PageParserResult page = target.Parse(file);
			var view = page.Actions["GET"].Views.Single();
			Assert.AreEqual(viewType, view.Component.Type);

			var content = (string)view.Properties["Content"].Value.Value;
			Assert.AreEqual("Hello", content);
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
