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
	public class XHtmlParserService_Test
    {
		[TestMethod]
		public void It_Should_be_possible_to_parse_xhtml()
		{
			var viewType = typeof(XHtmlView);

            string resourceLocation = "~/item.xrc.xhtml";
			var expectedContent = new XDocument(new XElement("test"));

			var viewCatalog = new Mocks.ViewCatalogServiceMock(new ComponentDefinition(viewType.Name, viewType));
			var pageProvider = new Mock<IResourceProviderService>();
            pageProvider.Setup(p => p.ResourceToXml(resourceLocation)).Returns(expectedContent);

			var target = new XHtmlParser(viewCatalog, pageProvider.Object);

            var page = target.Parse(resourceLocation);
			var view = page.Actions["GET"].Views.Single();
			Assert.AreEqual(viewType, view.Component.Type);

			var content = (XDocument)view.Properties["Content"].Value.Value;
			Assert.AreEqual(expectedContent, content);

		}
    }
}
