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
	public class XsltParserService_Test
    {
		[TestMethod]
		public void It_Should_be_possible_to_parse_xslt_with_xml()
		{
			var viewType = typeof(XsltView);

			var expectedContent = new XDocument(new XElement("test"));
			var expectedXml = new XDocument(new XElement("test"));

			var schemaParser = new Mock<IXrcSchemaParserService>();
			var viewCatalog = new Mocks.ViewCatalogServiceMock(new ComponentDefinition(viewType.Name, viewType));
			var pageProvider = new Mock<IResourceProviderService>();
			pageProvider.Setup(p => p.ResourceToXml("~/item.xrc.xslt")).Returns(expectedContent);
			pageProvider.Setup(p => p.ResourceToXml("~/item.xml")).Returns(expectedXml);
			pageProvider.Setup(p => p.ResourceExists("~/item.xml")).Returns(true);

			var target = new XsltParserService(schemaParser.Object, viewCatalog, pageProvider.Object);

			PageParserResult page = target.Parse(GetItem("item.xrc.xslt"));
			var view = page.Actions["GET"].Views.Single();
			Assert.AreEqual(viewType, view.Component.Type);

			var content = (XDocument)view.Properties["Xslt"].Value.Value;
			Assert.AreEqual(expectedContent, content);

			content = (XDocument)view.Properties["Data"].Value.Value;
			Assert.AreEqual(expectedXml, content);
		}

		[TestMethod]
		public void It_Should_be_possible_to_parse_xslt_without_xml()
		{
			var viewType = typeof(XsltView);

			var expectedContent = new XDocument(new XElement("test"));

			var schemaParser = new Mock<IXrcSchemaParserService>();
			var viewCatalog = new Mocks.ViewCatalogServiceMock(new ComponentDefinition(viewType.Name, viewType));
			var pageProvider = new Mock<IResourceProviderService>();

			pageProvider.Setup(p => p.ResourceToXml("~/conventions_xslt.xrc.xslt")).Returns(expectedContent);
			pageProvider.Setup(p => p.ResourceExists("~/conventions_xslt.xml")).Returns(false);

			var target = new XsltParserService(schemaParser.Object, viewCatalog, pageProvider.Object);

			PageParserResult page = target.Parse(GetItem("conventions_xslt.xrc.xslt"));
			var view = page.Actions["GET"].Views.Single();
			Assert.AreEqual(viewType, view.Component.Type);

			var content = (XDocument)view.Properties["Xslt"].Value.Value;
			Assert.AreEqual(expectedContent, content);
		}

		private XrcItem GetItem(string fileName)
		{
			var item = XrcItem.NewXrcFile(fileName);
			var xrcRoot = XrcItem.NewRoot("~", item);

			return item;
		}
    }
}
