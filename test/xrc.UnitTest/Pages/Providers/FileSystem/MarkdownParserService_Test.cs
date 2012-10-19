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
using xrc.Modules;
using System.IO;
using xrc.Pages.Providers.Common.Parsers;
using xrc.Pages.Providers.Common;

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
			var viewType = typeof(HtmlView);

			var expectedContent = "## Test";

			var schemaParser = new Mock<IXrcSchemaParserService>();
			var viewCatalog = new Mocks.ViewCatalogServiceMock(new ComponentDefinition(viewType.Name, viewType));
			var pageProvider = new Mock<IPageProviderService>();
			pageProvider.Setup(p => p.ResourceToText("~/item.xrc.md")).Returns(expectedContent);

			var target = new MarkdownParserService(schemaParser.Object, viewCatalog, pageProvider.Object);

			PageParserResult page = target.Parse(GetItem("item.xrc.md"));
			var view = page.Actions["GET"].Views.Single();
			Assert.AreEqual(viewType, view.Component.Type);

			var content = (string)view.Properties["Content"].Value.Value;
			Assert.AreEqual(expectedContent, content);
		}

		private XrcItem GetItem(string fileName)
		{
			var xrcRoot = XrcItem.NewRoot("root");
			return XrcItem.NewXrcFile(xrcRoot, "id", fileName);
		}
    }
}
