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
	public class HtmlParserService_Test
    {
        [TestInitialize]
        public void Init()
        {
        }
		[TestMethod]
		public void It_Should_be_possible_to_parse_html()
		{
			var viewType = typeof(HtmlView);

			var expectedContent = "<h1>test</h1>";
            string resourceLocation = "~/item.xrc.html";

			var viewCatalog = new Mocks.ViewCatalogServiceMock(new ComponentDefinition(viewType.Name, viewType));
			var pageProvider = new Mock<IResourceProviderService>();
            pageProvider.Setup(p => p.ResourceToHtml(resourceLocation)).Returns(expectedContent);

			var target = new HtmlParser(viewCatalog, pageProvider.Object);

            PageDefinition page = target.Parse(resourceLocation);
			var view = page.Actions["GET"].Views.Single();
			Assert.AreEqual(viewType, view.Component.Type);

			var content = (string)view.Properties["Content"].Value.Value;
			Assert.AreEqual(expectedContent, content);
		}
	}
}
