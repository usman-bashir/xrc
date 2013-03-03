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
using xrc.Pages.Providers;

namespace xrc.Pages.Parsers
{
	[TestClass]
	public class XsltParserService_Test
    {
		[TestMethod]
		public void It_Should_be_possible_to_parse_xslt_with_xml()
		{
			var viewType = typeof(XsltView);

            string resourceLocation = "~/item.xrc.xslt";

			var expectedContent = new XDocument(new XElement("test"));
			var expectedXml = new XDocument(new XElement("test"));

			var viewCatalog = new Mocks.ViewCatalogServiceMock(new ComponentDefinition(viewType.Name, viewType));
			var pageProvider = new Mock<IResourceProviderService>();
            pageProvider.Setup(p => p.ResourceToXml(resourceLocation)).Returns(expectedContent);
			pageProvider.Setup(p => p.ResourceToXml("~/item.xml")).Returns(expectedXml);
			pageProvider.Setup(p => p.ResourceExists("~/item.xml")).Returns(true);

			var target = new XsltParserService(viewCatalog, pageProvider.Object);

            PageDefinition page = target.Parse(resourceLocation);
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
            string resourceLocation = "~/item.xrc.xslt";

			var viewCatalog = new Mocks.ViewCatalogServiceMock(new ComponentDefinition(viewType.Name, viewType));
			var pageProvider = new Mock<IResourceProviderService>();

			pageProvider.Setup(p => p.ResourceToXml(resourceLocation)).Returns(expectedContent);
			pageProvider.Setup(p => p.ResourceExists("~/item.xml")).Returns(false);

			var target = new XsltParserService(viewCatalog, pageProvider.Object);

            PageDefinition page = target.Parse(resourceLocation);
			var view = page.Actions["GET"].Views.Single();
			Assert.AreEqual(viewType, view.Component.Type);

			var content = (XDocument)view.Properties["Xslt"].Value.Value;
			Assert.AreEqual(expectedContent, content);
		}
    }
}
