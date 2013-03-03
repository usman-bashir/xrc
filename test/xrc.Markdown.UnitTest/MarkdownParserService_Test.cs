using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using xrc.Views;
using xrc.Script;
using Moq;
using xrc.Modules;
using System.IO;
using xrc.Pages.Providers;

namespace xrc.Pages.Parsers
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
			var viewType = typeof(MarkdownView);

			var expectedContent = "## Test";
            string resourceLocation = "~/item.xrc.md";

            var viewCatalog = new Mock<IViewCatalogService>(); 
            viewCatalog.Setup(p => p.Get(viewType.Name)).Returns(new ComponentDefinition(viewType.Name, viewType));
			var pageProvider = new Mock<IResourceProviderService>();
            pageProvider.Setup(p => p.ResourceToText(resourceLocation)).Returns(expectedContent);

			var target = new MarkdownParserService(viewCatalog.Object, pageProvider.Object);

            PageDefinition page = target.Parse(resourceLocation);
			var view = page.Actions["GET"].Views.Single();
			Assert.AreEqual(viewType, view.Component.Type);

			var content = (string)view.Properties["Content"].Value.Value;
			Assert.AreEqual(expectedContent, content);
		}
    }
}
