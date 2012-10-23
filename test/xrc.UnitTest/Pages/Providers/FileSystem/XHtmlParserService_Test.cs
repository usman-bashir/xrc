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
	public class XHtmlParserService_Test
    {
		[TestMethod]
		public void It_Should_be_possible_to_parse_xhtml()
		{
			var viewType = typeof(XHtmlView);

			var expectedContent = new XDocument(new XElement("test"));

			var schemaParser = new Mock<IXrcSchemaParserService>();
			var viewCatalog = new Mocks.ViewCatalogServiceMock(new ComponentDefinition(viewType.Name, viewType));
			var pageProvider = new Mock<IResourceProviderService>();
			pageProvider.Setup(p => p.ResourceToXml("~/item.xrc.xhtml")).Returns(expectedContent);

			var target = new XHtmlParserService(schemaParser.Object, viewCatalog, pageProvider.Object);

			var page = target.Parse(GetItem("item.xrc.xhtml"));
			var view = page.Actions["GET"].Views.Single();
			Assert.AreEqual(viewType, view.Component.Type);

			var content = (XDocument)view.Properties["Content"].Value.Value;
			Assert.AreEqual(expectedContent, content);

		}

		[TestMethod]
		public void It_Should_be_possible_to_parse_xhtml_page_with_layout()
		{
			var viewType = typeof(XHtmlView);

			var item = XrcItem.NewXrcFile("item.xrc.xhtml");
			var layout = XrcItem.NewXrcFile("_layout.xrc");
			var shared = XrcItem.NewDirectory("shared", layout);
			var xrcRoot = XrcItem.NewRoot("~", shared, item);

			var expectedContent = new XDocument(new XElement("test"));

			var schemaParser = new Mock<IXrcSchemaParserService>();
			var viewCatalog = new Mocks.ViewCatalogServiceMock(new ComponentDefinition(viewType.Name, viewType));
			var pageProvider = new Mock<IResourceProviderService>();
			pageProvider.Setup(p => p.ResourceToXml("~/item.xrc.xhtml")).Returns(expectedContent);

			var target = new XHtmlParserService(schemaParser.Object, viewCatalog, pageProvider.Object);

			var page = target.Parse(item);
			var action = page.Actions["GET"];
			var view = action.Views.Single();
			Assert.AreEqual(viewType, view.Component.Type);

			var content = (XDocument)view.Properties["Content"].Value.Value;
			Assert.AreEqual(expectedContent, content);

			Assert.AreEqual("~/shared/_layout", action.Layout);
		}

		private XrcItem GetItem(string fileName)
		{
			var item = XrcItem.NewXrcFile(fileName);
			var xrcRoot = XrcItem.NewRoot("~", item);

			return item;
		}
    }
}
